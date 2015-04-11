using UnityEngine;
using System.Collections;
using System;

public enum CarryType { None, Food};

/// <summary>
/// This class is used to represent an ant unit in the game.
/// </summary>
public class Ant : MonoBehaviour , IEquatable<Ant>
{
	/// <summary>
	/// The time for an ant to move 1 unit
	/// </summary>
	public const float moveTime = .5f;

	/// <summary>
	/// The amount of energy the ant the ant uses a second
	/// </summary>
	public const float energyUsageSec = .25f;

	/// <summary>
	/// The carry capacity of the ant.
	/// </summary>
	public const float carryCapacity = 10f;

	/// <summary>
	/// The carry capacity of the ant.
	/// </summary>
	public const float foodEnegryCapacity = 1f;

	/// <summary>
	/// The type stuff the ant is carrying.
	/// </summary>
	public CarryType carryType;

	/// <summary>
	/// The time in seconds between ant updates
	/// </summary>
	public const float SecForAntUpdate = .5f;

	/// <summary>
	/// The energy usage per ant update.
	/// </summary>
	protected const float EnergyUsedPerAntUpdate = energyUsageSec*SecForAntUpdate;

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
	protected bool findingFood;

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
	/// The sprite renderer.
	/// </summary>
	protected SpriteRenderer spriteRender;

	/// <summary>
	/// The 2D rigid body for the ant
	/// </summary>
	protected Rigidbody2D rb2D;	

	/// <summary>
	/// Initialize the ant gameobject, this should be called
	/// after instantiation.
	/// </summary>
	public virtual void Init()
	{
		_antID = GameManager.GetNextAntID();
		currentOrder = "";
		assigned = false;
		inverseMoveTime = 1f / moveTime;
		foodEnergy = foodEnegryCapacity;
		carryAmount = 0f;
		carryType = CarryType.None;
		findingFood = false;
	}

	// Use this for initialization
	protected virtual void Start () 
	{
	
		// If init was not called for this object yet, call it
		if(_antID == 0)
		{
			Init();
		}

		//Get a component references
		rb2D = GetComponent <Rigidbody2D> ();
		spriteRender = GetComponent <SpriteRenderer> ();

		// Ant starts in the nest
		inNest = true;
		Hide();

		// Start the ant update
		StartCoroutine(AntUpdate());
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
			// Determine food take location
			float energyDeficiet = 0f;
			if(inNest) {
				energyDeficiet = EnergyUsedPerAntUpdate - NetworkManager.instance.nest.TakeFood(EnergyUsedPerAntUpdate);
			}

			if(foodEnergy > 0)
			{
				foodEnergy -= energyDeficiet;
			}
			else if(findingFood)
			{
				foodEnergy = 0;
			}
			else
			{
				foodEnergy = 0;
				findingFood = true;
			}

			yield return new WaitForSeconds(SecForAntUpdate);
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

		findingFood = false;
		
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
