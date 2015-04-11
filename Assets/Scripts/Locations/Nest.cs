using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This calls represents the Nest game location
/// </summary>
public class Nest : Location {

	protected const int _startingWorkerAnts = 100;

	/// <summary>
	/// This initializes this instance, for intializaing internal objects 
	/// before Awake() is called.
	/// </summary>
	public override void Init()
	{
		base.Init();

		// Create the initial workers
		CreateWorkerAnts(_startingWorkerAnts);
		locationText.text = WorkerAntCount + " Workers";
	}

	protected override void Awake()
	{
		base.Awake();
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
		int checkAmount = 0;
		for(int i = 0; i < takeAmount && checkAmount < WorkerAntCount; i++)
		{
			// Only take idle workers
			if(workerAntQ.Peek().IsIdle)
			{
				retList.Add(workerAntQ.Dequeue());
			}
			checkAmount++;
		}

		// Update worker count
		locationText.text = WorkerAntCount + " Workers";
		return retList;
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
			// Add to q
			workerAntQ.Enqueue((WorkerAnt)antAdding);

			// Update worker count
			locationText.text = WorkerAntCount + " Workers";
		}
		else
		{
			Debug.Log("AddToNest: ERROR - unknown ant morph adding to nest");
		}
	}

	/// <summary>
	/// This function creates the given number of worker ants and adds them to the nest.
	/// </summary>
	/// <param name="amount">The desired amount of worker ants to create.</param>
	public void CreateWorkerAnts(int amount)
	{
		WorkerAnt tempAnt;
		for(int i = 0; i < amount; i++)
		{
			tempAnt = PrefabManager.instance.CreateWorkerAntObject(transform.position);
			workerAntQ.Enqueue(tempAnt);
			tempAnt.transform.parent = transform;
		}
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
	}

	/// <summary>
	/// This couroutine function allows an ant to exit a location. This couroutine should be called 
	/// by the ant when the ant wants to leave a node.
	/// </summary>
	/// <param name="exitingAnt">The ant that is exiting the location</param>
	public override IEnumerator Exit(Ant exitingAnt)
	{
		exitingAnt.Unhide();

		// Currently just a random wait
		float waitTime = Random.Range(0.1f, 1f);
		yield return new WaitForSeconds(waitTime);
	}
}
