using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This calls represents the Nest game location
/// </summary>
public class Nest : Location {

	public const int startingWorkerAnts = 20;
	public const int startWorkAntPool = 100;
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

	/// <summary>
	/// This is a pool of worker ant objects to pull from when birthing ants
	/// </summary>
	protected Queue<WorkerAnt> workerAntPool;

	/// <summary>
	/// This initializes this instance, for intializaing internal objects 
	/// before Awake() is called.
	/// </summary>
	public override void Init()
	{
		base.Init();

		foodStorage = 1000f;
		workerAntPool = new Queue<WorkerAnt>();

		// Add starting ants to pool
		WorkerAnt tempAnt;
		for(int i = 0; i < startWorkAntPool; i++)
		{
			tempAnt = PrefabManager.instance.CreateWorkerAntObject(transform.position);
			workerAntPool.Enqueue(tempAnt);
			tempAnt.transform.parent = transform;
		}

		upperText.text = WorkerAntCount + " Workers";
		lowerText.text = ((int)foodStorage) + " Food Stored";
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start ()
	{
		base.Start ();

		// Birth the initial workers
		BirthWorkerAnts(startingWorkerAnts);

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
        float antBirthLeftover = 0f;
        int antBirthThisUpdate;

        float local_antBirthPerSec = antBirthPerSec;
        float antBirthPerUpdate = local_antBirthPerSec*secPerNestUpdate;
        float foodFromBirthThisUpdate = antBirthPerUpdate*foodPerAntBirth;

		for(;;)
		{
            // Update calculations when antBirthPerSec changes
            if(local_antBirthPerSec != antBirthPerSec)
            {
                local_antBirthPerSec = antBirthPerSec;
                antBirthPerUpdate = local_antBirthPerSec*secPerNestUpdate;
                foodFromBirthThisUpdate = antBirthPerUpdate*foodPerAntBirth;
            }

            antBirthThisUpdate = Mathf.FloorToInt(antBirthPerUpdate + antBirthLeftover);
            antBirthLeftover = (antBirthPerUpdate + antBirthLeftover) - (float) antBirthThisUpdate;

			if(foodStorage > (foodReserveLevel + foodFromBirthThisUpdate))
			{
                foodStorage -= foodFromBirthThisUpdate;
                BirthWorkerAnts(antBirthThisUpdate);

			}
            else if(foodStorage >= baseFoodPerUpdate)
            {
                foodStorage -= baseFoodPerUpdate;
            }
            else
            {
                Debug.Log("The Queen Has Starved!");
                yield break;
            }

            lowerText.text = ((int)foodStorage) + " Food Stored";

			yield return new WaitForSeconds(secPerNestUpdate);
		}
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

	/// <summary>
	/// This function removes workers up to the paramter, if able, 
	/// and returns a list of the worker ants.
	/// </summary>
	/// <returns>A list of worker ants taken from the nest.</returns>
	/// <param name="amount">The amount of workers to try and remove from
	/// the Nest.</param>
	public List<WorkerAnt> TakeWorkerAnts(int amount)
	{
		List<WorkerAnt> retList = new List<WorkerAnt>();

		if(amount < 0)
		{
			return retList;
		}

		int takeAmount = 0;
		if(WorkerAntCount >= amount)
		{
			takeAmount = amount;
		}
		else
		{
			takeAmount = WorkerAntCount;
		}

		// Move ants to return queue
		int startingCount = WorkerAntCount;
		int foundAmount = 0;
		for(int i = 0; i < startingCount; i++)
		{
			// Only take idle workers
			if(workerAntList[WorkerAntCount - 1].IsIdle)
			{
				retList.Add(workerAntList[WorkerAntCount - 1]);
				workerAntList.RemoveAt(WorkerAntCount - 1);
				if(++foundAmount == takeAmount)
				{
					break;
				}
			}
		}

		// Update worker count
		upperText.text = WorkerAntCount + " Workers";
		return retList;
	}

	/// <summary>
	/// Takes food from the food storage. Zero food is returned if the
	/// nest food storage is below reserve levels.
	/// </summary>
	/// <returns>The amount of food taken</returns>
	/// <param name="amount">The amount of food attempted to take.</param>
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
		lowerText.text = ((int)foodStorage) + " Food Stored";

		return takeAmount;
	}

	/// <summary>
	/// This adds the ant parameter to the Nest. The ant enters the proper
	/// queue for its morphology.
	/// </summary>
	/// <param name="antAdding">The ant object entering the nest</param>
	public void AddToNest(Ant antAdding)
	{
		// Sort by morphology
		if(antAdding is WorkerAnt)
		{
			// Add to list
			workerAntList.Insert(0, (WorkerAnt)antAdding);

			// Update worker count
			upperText.text = WorkerAntCount + " Workers";
		}
		else
		{
			Debug.Log("AddToNest: ERROR - unknown ant morph adding to nest");
		}
	}

	/// <summary>
	/// This function births a given number of worker ants. If ants exist
	/// in the ant pool, it prioritizes taking those. Otherwise, it creates new
	/// ant objects
	/// </summary>
	/// <param name="amount">The desired amount of worker ants to birth.</param>
	public void BirthWorkerAnts(int amount)
	{
		if(amount < 0)
		{
			return;
		}

		WorkerAnt tempAnt;
		for(int i = 0; i < amount; i++)
		{
			if(workerAntPool.Count != 0)
			{
				tempAnt = workerAntPool.Dequeue();
			}
			else
			{
				tempAnt = PrefabManager.instance.CreateWorkerAntObject(transform.position);
				tempAnt.transform.parent = transform;
			}

			tempAnt.Birth();
			workerAntList.Insert(0, tempAnt);
		}

		upperText.text = WorkerAntCount + " Workers";
	}

	/// <summary>
	/// This function handles the death of an ant. It pulls the ant from the location it's
	/// at and adds it back to the pool.
	/// </summary>
	/// <param name="deadAnt">The newly dead ant.</param>
	public void HandleAntDeath(Ant deadAnt)
	{
		deadAnt.assignedLocation.RemoveAnt(deadAnt);
		if(deadAnt is WorkerAnt)
		{
			workerAntPool.Enqueue((WorkerAnt)deadAnt);
		}
		else
		{
			Debug.Log("HandleAntDeath: ERROR - Unknown ant class");
		}
		deadAnt.transform.position = transform.position;
	}

	/// <summary>
	/// This couroutine function allows an ant to enter a location. This couroutine should be called 
	/// by the ant when the ant wants to enter a node. If the entering ant is idle, the
	/// ant is added to the nest.
	/// </summary>
	/// <param name="enteringAnt">The ant that is entering the location</param>
	public override IEnumerator Enter(Ant enteringAnt)
	{
		enteringAnt.Hide();

		// Currently just a random wait
		float waitTime = Random.Range(0.1f, 1f);
		yield return new WaitForSeconds(waitTime);

		// If the ant is idle, add it to the nest
		if(enteringAnt.IsIdle)
		{
			AddToNest(enteringAnt);
		}

		// If ant is carrying food, give it to the nest
		float giveAmount = 0f;
		if(enteringAnt.carryType == CarryType.Food)
		{
			giveAmount = enteringAnt.GiveAll();
			foodStorage += giveAmount;
		}

		// Eat if it needs to
		float eatAmount = enteringAnt.eat(foodStorage);
		foodStorage -= eatAmount;
		if(eatAmount != 0 || giveAmount != 0)
		{
			lowerText.text = ((int)foodStorage) + " Food Stored";
		}

		enteringAnt.inNest = true;
	}

	/// <summary>
	/// This couroutine function allows an ant to exit a location. This couroutine should be called 
	/// by the ant when the ant wants to leave a node.
	/// </summary>
	/// <param name="exitingAnt">The ant that is exiting the location</param>
	public override IEnumerator Exit(Ant exitingAnt)
	{
		exitingAnt.Unhide();

		// Eat if it needs to
		float eatAmount = exitingAnt.eat(foodStorage);
		foodStorage -= eatAmount;
		if(eatAmount != 0f) 
		{
			lowerText.text = ((int)foodStorage) + " Food Stored";
		}

		// Currently just a random wait
		float waitTime = Random.Range(0.1f, 1f);
		yield return new WaitForSeconds(waitTime);

		exitingAnt.inNest = false;
	}
}
