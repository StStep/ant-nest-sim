using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public enum CarryType { None, Food};

/// <summary>
/// This class is used to represent an ant unit in the game.
/// </summary>
public class Ant : MonoBehaviour , IEquatable<Ant>
{
	public const float timeToMoveOneUnit = .5f;
	public const float carryCapacity = 10f;
	public const float foodEnegryCapacity = 1f;
	public const float minLifespan = 20f;
	public const float maxLifespan = 40f;
    public const float secPerAntUpdate = .5f;
    public const float energyUsageSec = .25f;
    protected const float EnergyUsedPerAntUpdate = energyUsageSec*secPerAntUpdate;

	/// <summary>
	/// The type stuff the ant is carrying.
	/// </summary>
	public CarryType carryType;		

	/// <summary>
	/// The location that this ant is assigned to
	/// </summary>
	public Location assignedLocation;

	/// <summary>
	/// Gets a value that is false if the ant is under an order, true if it
	/// is waiting in the nest for a new order.
	/// </summary>
	/// <value>Gets if the currentOrder string is null.</value>
	public bool IsIdle
	{
		get
		{
			return String.IsNullOrEmpty(currentOrder);
		}
	}

	/// <summary>
	/// This is true if the ant is in a nest. 
	/// </summary>
	public bool inNest;

	/// <summary>
	/// This variable is true if the ant is currently assigned to a location. If false
	/// the ant is either making it's way back to the nest, or in the nest.
	/// </summary>
	[HideInInspector]public bool assigned;

	/// The ant ID, unique for each <see cref="Ant"/> object
	protected int _antID;
	/// <summary>
	/// The ant ID, unique for each <see cref="Ant"/> object
	/// </summary>
	/// <value>The ant ID.</value>
	public int AntID
	{
		get
		{
			return _antID;
		}
	}

	/// <summary>
	/// Gets a value indicating whether the ant is full to capcaity.
	/// </summary>
	/// <value><c>true</c> if this instance is full; otherwise, <c>false</c>.</value>
	public bool IsFull
	{
		get
		{
			return ((carryCapacity - carryAmount) < float.Epsilon) ? true : false;
		}
	}

    /// <summary>
    /// The current amount being carried by the ant.
    /// </summary>
    protected float carryAmount;
    
    /// <summary>
    /// The amount of energy due to food the ant has
    /// </summary>
    protected float foodEnergy;
    
    /// <summary>
    /// The inverse move time, pre-calculated for efficiency
    /// </summary>
    protected float inverseMoveTime;
    
    /// <summary>
    /// The current order the ant is following.
    /// </summary>
    protected string currentOrder;
    
    /// <summary>
    /// If the ant is trying to find food
    /// </summary>
    protected bool isStarving;

	/// <summary>
	/// The sprite renderer.
	/// </summary>
	protected SpriteRenderer spriteRender;

	/// <summary>
	/// The 2D rigid body for the ant
	/// </summary>
	protected Rigidbody2D rb2D;	

	/// <summary>
	/// The calculated lifespan for the ant in seconds
	/// </summary>
	protected float lifeSpan;

	/// <summary>
	/// The time in seconds the and has been alive
	/// </summary>
	protected float timeAlive;

	/// <summary>
	/// Initialize the ant gameobject, this should be called
	/// after instantiation.
	/// </summary>
	public virtual void Init()
	{
		_antID = GameManager.GetNextAntID();
		inverseMoveTime = 1f / timeToMoveOneUnit;
		enabled = false;
		ResetState();
	}

	protected void ResetState()
	{
		currentOrder = "";
		assigned = false;
		foodEnergy = foodEnegryCapacity;
		carryAmount = 0f;
		carryType = CarryType.None;
		isStarving = false;
		lifeSpan = Random.Range(minLifespan, maxLifespan);
		timeAlive = 0f;
		assignedLocation = null;
		inNest = false;
	}

	/// <summary>
	/// This function starts the ant processes. This should
	/// be called when reusing the ant from a ant object pool.
	/// <remarks>This function can only be called after the ant object has
	/// passed its awake() call.</remarks>
	/// </summary>
	public virtual void Birth()
	{
		enabled = true;

		// Reset state
		ResetState();

		// Ant starts in the nest
		inNest = true;
		assignedLocation = NetworkManager.instance.nest;
		Hide();
		
		// Start the ant update
		StartCoroutine(AntUpdate());
	}

	// Use this for initialization
	protected virtual void Awake () 
	{
	
		// If init was not called for this object yet, call it
		if(_antID == 0)
		{
			Init();
		}

		//Get a component references
		rb2D = GetComponent <Rigidbody2D> ();
		spriteRender = GetComponent <SpriteRenderer> ();
	}
	
	// Update is called once per frame
	protected virtual void Update () 
	{

	}

	/// <summary>
	/// This coroutine is used for updating the state of the ant. It happends less often
	/// then Update, and is for a coarser update schedule for ant specific states.
	/// </summary>
	/// <returns>Returns IEnumerator, intended to be used as a coroutine.</returns>
	protected virtual IEnumerator AntUpdate()
	{
		for(;;)
		{
			// Check lifespan
			timeAlive += secPerAntUpdate;
			if(timeAlive >= lifeSpan)
			{
				this.Die();
			}

			// Determine food take location
			float energyDeficiet = 0f;
			if(inNest) {
				energyDeficiet = EnergyUsedPerAntUpdate - NetworkManager.instance.nest.TakeFood(EnergyUsedPerAntUpdate);
			}

			// When starving, the ant's lifetime is shortened for every second it starves
			if(foodEnergy > 0)
			{
				foodEnergy -= energyDeficiet;
			}
			else if(isStarving)
			{
				lifeSpan -= secPerAntUpdate;
				foodEnergy = 0;
			}
			else
			{
				lifeSpan -= secPerAntUpdate;
				foodEnergy = 0;
				isStarving = true;
			}

			yield return new WaitForSeconds(secPerAntUpdate);
		}
	}

	/// <summary>
	/// This function causes the ant to be visible in the game space.
	/// </summary>
	public void Hide()
	{
		spriteRender.enabled = false;
	}

	/// <summary>
	/// This function causes the ant to no longer be visible in the game space.
	/// </summary>
	public void Unhide()
	{
		spriteRender.enabled = true;
	}

	/// <summary>
	/// This function causes this ant gameobject to die, stopping
	/// all coroutines and removing it from locations.
	/// </summary>
	public void Die()
	{
		StopAllCoroutines();
		NetworkManager.instance.nest.HandleAntDeath(this);
		Hide();
		enabled = false;
	}

	/// <summary>
	/// The ant tries to take the given amount of CarryType. It will only carry
	/// more of the current carry type, and will not go above it's capacity.
	/// </summary>
	/// <param name="amount">The total available stuff the ant could carry.</param>
	/// <returns>The amount the ant takes.</returns>
	public float Take(CarryType type, float amount)
	{
		// Only take more of the current carry type
		if((type != carryType && carryType != CarryType.None) || (amount < 0f))
		{
			return 0f;
		}

		// Take as much as is available and the ant can carry
		carryType = type;
		float takeAmount;
		if(amount >= (carryCapacity - carryAmount))
		{
			takeAmount = (carryCapacity - carryAmount);
			carryAmount = carryCapacity;
		}
		else
		{
			takeAmount = amount;
			carryAmount = carryAmount + amount;
		}

		return takeAmount;
	}

	/// <summary>
	/// Eat the specified amount.
	/// </summary>
	/// <param name="amount">The total amount of food available to be eaten.</param>
	public float eat(float amount)
	{		
		if(amount < 0)
		{
			return 0f;
		}

		// Eat as much as the ant can
		float eatAmount;
		if(amount >= (foodEnegryCapacity - foodEnergy))
		{
			eatAmount = (foodEnegryCapacity - foodEnergy);
			foodEnergy = foodEnegryCapacity;
		}
		else
		{
			eatAmount = amount;
			foodEnergy = foodEnergy + amount;
		}

		isStarving = false;
		
		return eatAmount;
	}

	/// <summary>
	/// The ant gives up everything it is carrying, and returns to carrying nothing.
	/// </summary>
	/// <returns>The amount of the current ant carry type given.</returns>
	public float GiveAll()
	{
		float amount = carryAmount;
		carryAmount = 0f;
		carryType = CarryType.None;
		return amount;
	}

	#region Coroutine Actions

	/// <summary>
	/// The action coroutine that moves the gameobject to a vector
	/// </summary>
	/// <returns>Returns IEnumerator, intended to be used as a coroutine.</returns>
	/// <param name="targetPosition">The position to move the gameobject to.</param>
	protected IEnumerator ActMoveToPosition(Vector2 targetPosition)
	{
		Vector2 diff;
		diff.x = targetPosition.x - transform.GetPositionX();
		diff.y = targetPosition.y - transform.GetPositionY();
		float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		if(angle < 0) angle += 360;

		// TODO Make this look nice, splines perhaps?
		//print ("Starting to rotate");
		/*		while (Mathf.Abs(transform.GetAngleZ() - targetAngle) > .1f)
		{
			float angle = Mathf.MoveTowardsAngle(transform.GetAngleZ(), targetAngle, rotateSpeed * Time.deltaTime);
			transform.SetAngleZ(angle);
			yield return 0;
		}*/
		
		transform.SetAngleZ(angle);

		//Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
		//Square magnitude is used instead of magnitude because it's computationally cheaper.
		Vector3 end = targetPosition.ToVec3(transform.GetPositionZ());
		float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
		
		//While that distance is greater than a very small amount (Epsilon, almost zero):
		while(sqrRemainingDistance > float.Epsilon)
		{
			//Find a new position proportionally closer to the end, based on the moveTime
			Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
			
			//Call MovePosition on attached Rigidbody2D and move it to the calculated position.
			rb2D.MovePosition (newPostion);
			
			//Recalculate the remaining distance after moving.
			sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			
			//Return and loop until sqrRemainingDistance is close enough to zero to end the function
			yield return null;
		}
	}

	#endregion

	/// <summary>
	/// Determines whether the specified <see cref="Ant"/> is equal to the current <see cref="Ant"/>.
	/// </summary>
	/// <param name="ant">The <see cref="Ant"/> to compare with the current <see cref="Ant"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="Ant"/> is equal to the current <see cref="Ant"/>; otherwise, <c>false</c>.</returns>
	public bool Equals(Ant ant)
	{
		if (ant == null) return false;
		return (this.AntID.Equals(ant.AntID));
	}
}
