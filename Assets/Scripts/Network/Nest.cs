using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This calls represents the Nest game location
/// </summary>
public class Nest : Location {

    public const int startingWorkerAnts = 20;
    public const float secOfFoodToReserve = 5f;
	public const float secPerNestUpdate = .5f;
	public const float baseFoodUsePerSec = 1f;
    public const float baseFoodPerUpdate = baseFoodUsePerSec*secPerNestUpdate;
    public const float foodPerAntBirth = .25f;
    public const float foodReserveLevel = secOfFoodToReserve * baseFoodUsePerSec;

    /// <summary>
    /// The amount of ants born a second
    /// </summary>
    public float antBirthPerSec;

	/// <summary>
	/// The current food, edible by ants, in the nest
	/// </summary>
	protected float foodStorage;

	public int antsInNest;

	// Ant reuse pool
	public Queue<Ant> antReusePool;

	/// <summary>
	/// This initializes this instance, for intializaing internal objects 
	/// before Awake() is called.
	/// </summary>
	public override void Init()
	{
		base.Init();

		antReusePool = new Queue<Ant>();
		foodStorage = 1000f;
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start ()
	{
		base.Start ();

		// Start the Nest update
		StartCoroutine(NestUpdate());
	}

	/// <summary>
	/// This coroutine checks is used to manage the nest state. Ants are only
    /// birthed if the food storage is greater than the reserve. If the base food rate
    /// can't be taken from food storage, and queen dies, and this function exits.
	/// </summary>
	/// <returns>Returns IEnumerator, intended to be used as a coroutine.</returns>
	protected IEnumerator NestUpdate()
	{
        yield break;
	}

	/// <summary>
	/// This function should be called when a mouse clicks
	/// up after clicking on the game object.
	/// </summary>
	public override void ClickUp()
	{
		if (clickStatus == ClickType.LeftClick)
		{
			Debug.Log("You left clicked on a nest");
		}
		else if (clickStatus == ClickType.RightClick)
		{
			Debug.Log("You right clicked on a nest");
		}
	}

	protected override void LetVisit(Ant ant)
	{
		// Ants that visit the nest are subsummed into ant number
		antsInNest++;
		if(ant.foodCarrying > 0f)
		{
			foodStorage += ant.foodCarrying;
		}
		antReusePool.Enqueue(ant);

	}

	/// <summary>
	/// Takes food from the food storage. Zero food is returned if the
	/// nest food storage is below reserve levels.
	/// </summary>
	/// <returns>The amount of food taken</returns>
	/// <param name="amount">The amount of food attempted to take.</param>
	// TODO THis is old
	public float TakeFood(float amount)
	{
		if(amount < 0f || foodStorage <= foodReserveLevel)
		{
			return 0f;
		}

		float takeAmount = 0;
		if(amount < foodStorage)
		{
			takeAmount = amount;
			foodStorage -= takeAmount;
		}
		else
		{
			takeAmount = foodStorage;
			foodStorage = 0;
		}

		return takeAmount;
	}

	public void SendAntsTo(int amount, Location location)
	{
		int sendAmount;
		if(amount < 0)
		{
			sendAmount = 0;
		}
		else if(antsInNest < amount)
		{
			sendAmount = antsInNest;
			antsInNest = 0;
		}
		else {
			sendAmount = amount;
			antsInNest -= amount;
		}
		
		Ant tempAnt;
		for(int i = 0; i < sendAmount; i++)
		{
			// Either reuse old ant or make new
			if(antReusePool.Count > 0)
			{
				tempAnt = antReusePool.Dequeue();
				tempAnt.Reset(this,location);
			}
			else
			{
				tempAnt = new Ant(this,location);
			}

			Accept(tempAnt);
		}
	}

	protected override void UpdateText()
	{
		lowerText.text = ((int)foodStorage) + " Food Stored";
	}
}
