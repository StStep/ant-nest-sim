using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This class is used to represent an ant unit in the game.
/// </summary>
public class Ant : MonoBehaviour {

	/// <summary>
	/// The time for an ant to move 1 unit
	/// </summary>
	public float moveTime = .5f;

	/// <summary>
	/// The inverse move time, pre-calculated for efficiency
	/// </summary>
	protected float inverseMoveTime;

	/// <summary>
	/// The current order the ant is following.
	/// </summary>
	protected string currentOrder;

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
	/// The sprite renderer.
	/// </summary>
	public SpriteRenderer spriteRender;

	/// <summary>
	/// The 2D rigid body for the ant
	/// </summary>
	public Rigidbody2D rb2D;	

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
	}

	// Use this for initialization
	void Start () 
	{
	
		// If init was not called for this object yet, call it
		if(_antID == 0)
		{
			Init();
		}

		//Get a component references
		// rb2D = GetComponent <Rigidbody2D> (); TODO Why doesn't this work?
		//spriteRender = GetComponent <SpriteRenderer> ();

		// Start in the nest, not visible
		EnterNest();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// This function causes the ant to be visible in the game space.
	/// </summary>
	public void EnterNest()
	{
		spriteRender.enabled = false;
	}

	/// <summary>
	/// This function causes the ant to no longer be visible in the game space.
	/// </summary>
	public void ExitNest()
	{
		spriteRender.enabled = true;
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
}
