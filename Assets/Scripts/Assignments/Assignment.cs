using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Assignment: MonoBehaviour
{
	protected List<WorkerAnt> _assignedWorkers;

	protected AssignableLocation _assignedLoc;
	public virtual AssignableLocation AssignedLocation
	{
		get
		{
			return _assignedLoc;
		}
	}

    public virtual int Count
    {
        get
		{
			return _assignedWorkers.Count;
		}
    }

	public virtual int LocID
	{
		get
		{
			if(_assignedLoc == null)
			{
				return 0;
			}
			else
			{
				return _assignedLoc.LocID;
			}
		}
	}

    public virtual void Init()
    {
		_assignedWorkers = new List<WorkerAnt>();
		enabled = false;
    }

	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	// Update is called once per frame
    protected virtual void Update () {
	
	}

	public virtual void LocationConnect(AssignableLocation location)
	{
		enabled = true;
		_assignedLoc = location;
		UpdateAssignmentText();
	}
	
	public virtual void LocationDisconnect()
	{
		enabled = false;
		_assignedLoc.upperText.text = "";
		_assignedLoc = null;
	}

	protected virtual void UpdateAssignmentText()
	{
		_assignedLoc.upperText.text = Count + " Workers";
	}

	protected virtual void HandleAssignedWorker(WorkerAnt assignedAnt)
	{
		assignedAnt.Assign();
	}

	protected virtual bool CheckUnassignReq(WorkerAnt ant)
	{
		return true;
	}

	public virtual void AssignWorkers(List<WorkerAnt> workers)
	{
		foreach(WorkerAnt worker in workers)
		{
			_assignedWorkers.Add(worker);
			HandleAssignedWorker(worker);
		}
		
		UpdateAssignmentText();
	}
	
	/// <summary>
	/// This function removes workers up to the paramter, if able, 
	/// and returns a list of the worker ants.
	/// </summary>
	/// <returns>A list of worker ants taken from the assignment.</returns>
	/// <param name="amount">The amount of workers to try and remove from
	/// the assignment.</param>
	public virtual List<WorkerAnt> UnassignWorkers(int amount)
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
			// Only unassign ants that pass the requirement
			if(CheckUnassignReq(_assignedWorkers[Count - 1]))
			{
				retList.Add(_assignedWorkers[Count - 1]);
				_assignedWorkers.RemoveAt(Count - 1);
				if(++foundAmount == takeAmount)
				{
					break;
				}
			}
		}
		
		UpdateAssignmentText();
		
		return retList;
	}
}
