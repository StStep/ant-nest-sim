using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO Do assignments need to be gameobjects?

public abstract class Assignment: MonoBehaviour
{
	protected AssignableLocation _assignedLoc;
	public virtual AssignableLocation AssignedLocation
	{
		get
		{
			return _assignedLoc;
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

	protected int _antsPerSec = 0;
	public int AntsPerSec
	{
		get
		{
			return _antsPerSec;
		}

		// Calculate antsPerTick whenever antsPerSec is set
		protected set
		{
			_antsPerSec = value;
			_antsPerTick = _antsPerSec*GameManager.secondsPerGameTick;
		}
	}

	protected float _antsPerTick = 0f;
	protected float _leftOverPerTick = 0f;
	public int AntsPerTick
	{
		// This allows for partial ants per tick to build up over ticks to eventually become a whole number
		get
		{
			int retInt = Mathf.FloorToInt(_antsPerTick + _leftOverPerTick);
			_leftOverPerTick = (_antsPerTick + _leftOverPerTick) - retInt;
			return retInt;
		}
	}

    public virtual void Init()
    {
    }

	// Use this for initialization
	protected virtual void Start () 
	{
		enabled = false;
	}
	
	// Update is called once per frame
    protected virtual void Update () 
	{
	
	}

	public virtual void LocationConnect(AssignableLocation location)
	{
		enabled = true;
		_assignedLoc = location;
		AntsPerSec = 0;
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
		_assignedLoc.upperText.text = AntsPerSec + " Ants";
	}

	public virtual void AssignAnts(int amount)
	{
		if(amount > 0)
		{
			AntsPerSec += amount;
			UpdateAssignmentText();
		}

	}

	public virtual void UnassignAnts(int amount)
	{
		if(amount < 0)
		{
			return;
		}

		if(amount > AntsPerSec)
		{
			AntsPerSec = 0;
		}
		else
		{
			AntsPerSec -= amount;
		}

		UpdateAssignmentText();
	}
}
