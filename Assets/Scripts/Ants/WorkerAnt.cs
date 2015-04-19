using UnityEngine;
using System.Collections;

/// <summary>
/// This is the ant that collects food for the nest.
/// </summary>
public class WorkerAnt : Ant , IStreamAnt 
{
	protected Route _routeToFollow;

	protected int _routeIndex;

	protected bool _moveTowardNest;

	/// <summary>
	/// Initialize the ant gameobject, this should be called
	/// after instantiation.
	/// </summary>
	public override void Init()
	{
		base.Init();
	}

	/// <summary>
	/// This function restarts the ant processes. This should
	/// be called when reusing the ant from a ant object pool.
	/// </summary>
	public override void Birth()
	{
		base.Birth();
	}

	// Use this for initialization
	protected override void Awake() 
	{
		base.Awake();
	
	}
	
	// Update is called once per frame
	protected override void  Update() 
	{
		base.Update();
	}

	public void Assign(Route assignedRoute)
	{
		base.Assign();
		_routeToFollow = assignedRoute;

		// Start at the end of the route, at the nest
		_routeIndex = _routeToFollow.Count - 1;
		_moveTowardNest = false;
		_routeToFollow[_routeIndex].Exit(this);
	}

	public override void Unassign()
	{
		base.Unassign();
		//_routeToFollow = null;
	}

	public override void HandleLocationExit()
	{
		Location locExiting = _routeToFollow[_routeIndex];

		// Reverse direction at the end of the route
		if((_routeIndex == 0) && !_moveTowardNest)
		{
			_moveTowardNest = true;
		}
		else if((_routeIndex == (_routeToFollow.Count - 1)) && _moveTowardNest)
		{
			_moveTowardNest = false;
		}

		if(_moveTowardNest)
		{
			_routeIndex++;
		}
		else
		{
			_routeIndex--;
		}

		Location destLocation = _routeToFollow[_routeIndex];

		locExiting.TakePathTo(this, destLocation);
	}
}
