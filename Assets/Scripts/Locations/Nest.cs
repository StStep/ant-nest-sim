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

	// TODO Need to handle ants that haven't returned to nest yet from last assignment
	// Currently ants are not assigned when finishing an assigment, and
	// only return to be assigning when making it back to the nest
	// Therefore the count above the nest will be workers 'inside' of it

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
	/// This Adds the worker ant parameter to the Nest. Inteded to be called
	/// by worker ants once they finish an assignemt, and have returned to the nest.
	/// </summary>
	/// <param name="worker">The worker ant object entering the nest</param>
	public void GiveWorkerAntSelf(WorkerAnt worker)
	{
		// Add to q
		workerAntQ.Enqueue(worker);

		// Update worker count
		locationText.text = WorkerAntCount + " Workers";
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
			tempAnt = PrefabManager.instance.CreateWorkerAnt(transform.position);
			workerAntQ.Enqueue(tempAnt);
			tempAnt.transform.parent = transform;
		}
	}
}
