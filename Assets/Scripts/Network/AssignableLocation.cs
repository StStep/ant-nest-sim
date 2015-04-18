using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class is for locations where ants are assigned to.
/// </summary>
public abstract class AssignableLocation : Location {

	/// <summary>
	/// The amount of worker ants added or removed with each click
	/// </summary>
	protected const int _workerAddNumber = 10;
	
	/// <summary>
	/// A route object holding the path from this location to the nest
	/// </summary>
	protected Route routeToNest;
	
	/// <summary>
    /// A route object holding a path from the nest to this location
	/// </summary>
	protected Route routeToThis;
	
	/// <summary>
	/// This initializes this instance, for intializaing internal objects 
	/// before Awake() is called.
	/// </summary>
	public override void Init()
	{
		base.Init();
		
		routeToThis = null;
		routeToNest = null;
	}
	
	protected override void Awake()
	{
		base.Awake();
	}
	
	/// <summary>
	/// This function should be called when a mouse clicks
	/// up after clicking on the game object.
	/// </summary>
	/// <para>
	/// This function adds/removes workers to the location from
	/// the nest each time it is clicked. Left clicking adds
	/// works and right clicking removes them.
	/// </para>
	public override void ClickUp()
	{
		AttemptToRouteToNest();
		
		if (clickStatus == ClickType.LeftClick)
		{
			if(routeToNest != null && routeToThis != null)
			{
				List<WorkerAnt> takenAnts = NetworkManager.instance.nest.TakeWorkerAnts(_workerAddNumber);
				string orderName = "Streaming to Rotten Apple ID " + LocID.ToString();
				foreach(WorkerAnt ant in takenAnts)
				{
					ant.assigned = true;
					ant.assignedLocation = this;
					workerAntList.Insert(0, ant);
					
					ant.OrderToStream(orderName, routeToThis, routeToNest);
				}
				upperText.text = WorkerAntCount + " Workers";
			}
			else
			{
                Debug.Log ("Left clicked on Rotten Apple ID " + LocID.ToString() + " but no paths");
			}
		}
		else if (clickStatus == ClickType.RightClick)
		{
			if(routeToNest != null && routeToThis != null)
			{
				UnassignWorkerAnts(_workerAddNumber);
				upperText.text = WorkerAntCount + " Workers";
			}
			else
			{
                Debug.Log ("Right clicked on Rotten Apple ID " + LocID.ToString() + " but no paths");
			}
		}
	}
	
	/// <summary>
	/// Unassigns the worker ants and removes them from the worker ant queue
	/// </summary>
	/// <returns>The amount of worker ants unassigned.</returns>
	/// <param name="amount">The amount of ants to attempt to unassign.</param>
	protected virtual int UnassignWorkerAnts(int amount)
	{
		if(amount < 0)
		{
			return 0;
		}
		
		int unassignAmount = 0;
		if(WorkerAntCount >= amount)
		{
			unassignAmount = amount;
		}
		else
		{
			unassignAmount = WorkerAntCount;
		}
		
		// Unasign ants from queue
		WorkerAnt ant;
		for(int i = 0; i < unassignAmount; i++)
		{
			ant = workerAntList[WorkerAntCount - 1];
			workerAntList.RemoveAt(WorkerAntCount - 1);
			ant.assignedLocation = NetworkManager.instance.nest;
			ant.assigned = false;
		}
		
		// Update worker count
		upperText.text = WorkerAntCount + " Workers";
		return unassignAmount;
	}
	
    // TODO Fix this once route caching is in, should look for route update/removal, change
	
	/// <summary>
    /// This funcion finds the routes to the nest and back if they are
	/// not set yet.
	/// </summary>
	protected void AttemptToRouteToNest()
	{
        // Try to find a route to the this location
		if(routeToThis == null)
		{
			routeToThis = NetworkManager.instance.LocationNetwork.GetRoute(NetworkManager.instance.nest, this);
		}
		
        // If a route to the nest doesn't exists, and route to this does, create the reverse route
		if(routeToNest == null && routeToThis != null)
		{
			routeToNest = routeToThis.Clone();
			routeToNest.Reverse();
		}
	}
}
