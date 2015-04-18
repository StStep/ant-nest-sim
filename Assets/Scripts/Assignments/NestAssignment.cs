using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NestAssignment : Assignment 
{
	// TODO This inheritance from assignment is messy, I have to void out AssignableLocation related functions to
	// replace their use with nest specific things
	protected Nest _assignedNest;
	public override AssignableLocation AssignedLocation
	{
		get
		{
			return null;
		}
	}

	public override int LocID
	{
		get
		{
			if(_assignedNest == null)
			{
				return 0;
			}
			else
			{
				return _assignedNest.LocID;
			}
		}
	}
    
    public override void Init ()
    {
        base.Init ();
    }

    protected override void Start ()
    {
        base.Start ();
    }

	public void LocationConnect(Nest location)
	{
		enabled = true;
		_assignedNest = location;
		BirthWorkerAnts(Nest.startingWorkerAnts);
	}

	public override void LocationConnect(AssignableLocation location)
	{
	}
	
	public override void LocationDisconnect()
	{
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
        Queue<WorkerAnt> workerAntPool = GameManager.instance.workerAntPool;
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
			_assignedWorkers.Insert(0, tempAnt);
        }
        
		_assignedNest.upperText.text = Count + " Workers";
    }

	public override void AssignWorkers(List<WorkerAnt> workers)
	{
		foreach(WorkerAnt worker in workers)
		{
			_assignedWorkers.Add(worker);
			worker.Unassign(); // Nest assignment is inverse
		}
		
		_assignedNest.upperText.text = Count + " Workers";
	}

    /// <summary>
    /// This function removes workers up to the paramter, if able, 
    /// and returns a list of the worker ants.
    /// </summary>
    /// <returns>A list of worker ants taken from the nest.</returns>
    /// <param name="amount">The amount of workers to try and remove from
    /// the Nest.</param>
	public override List<WorkerAnt> UnassignWorkers(int amount)
    {
        List<WorkerAnt> retList = new List<WorkerAnt>();
        if(amount < 0)
        {
            return retList;
        }
        
        int takeAmount = 0;
        if(Count >= amount)
        {
            takeAmount = amount;
        }
        else
        {
            takeAmount = Count;
        }
        
        // Move ants to return queue
        int startingCount = Count;
        int foundAmount = 0;
        for(int i = 0; i < startingCount; i++)
        {
            // Only take idle workers
			if(_assignedWorkers[Count - 1].IsIdle)
            {
				retList.Add(_assignedWorkers[Count - 1]);
				_assignedWorkers.RemoveAt(Count - 1);
                if(++foundAmount == takeAmount)
                {
                    break;
                }
            }
        }
        
        // Update worker count
		_assignedNest.upperText.text = Count + " Workers";

        return retList;
    }
}
