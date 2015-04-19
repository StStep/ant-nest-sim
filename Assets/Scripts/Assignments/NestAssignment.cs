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
        
		UpdateAssignmentText();
    }

	protected override void HandleAssignedWorker(WorkerAnt assignedAnt)
	{
		assignedAnt.Unassign(); // Nest assignment is inverse
	}

	protected override void UpdateAssignmentText()
	{
		_assignedNest.upperText.text = Count + " Workers";
	}

	protected override bool CheckUnassignReq(WorkerAnt ant)
	{
		return ant.IsIdle;
	}
}
