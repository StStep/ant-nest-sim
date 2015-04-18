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

    public int Count
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
		_assignedLoc.upperText.text = Count + " Workers";
	}
	
	public virtual void LocationDisconnect()
	{
		enabled = false;
		_assignedLoc.upperText.text = "";
		_assignedLoc = null;
	}

	public virtual void AssignWorkers(List<WorkerAnt> workers)
	{
		foreach(WorkerAnt worker in workers)
		{
			_assignedWorkers.Add(worker);
			worker.Assign();
		}
		
		_assignedLoc.upperText.text = Count + " Workers";
	}
	
	public virtual List<WorkerAnt> UnassignWorkers(int amount)
	{
		List<WorkerAnt> retList = new List<WorkerAnt>();
		if(amount < 0)
		{
			return retList;
		}

		int takeAmount;
		if(Count >= amount)
		{
			takeAmount = amount;
		}
		else
		{
			takeAmount = Count;
		}

		for(int i = 0; i < takeAmount; i ++)
		{
			retList.Add(_assignedWorkers[Count - 1]);
			_assignedWorkers.RemoveAt(Count - 1);
		}

		// Update worker count
		_assignedLoc.upperText.text = Count + " Workers";
		
		return retList;
	}
}
