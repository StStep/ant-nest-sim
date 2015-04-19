using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StreamAssignment : Assignment
{
	protected Route _routeToNest;

    public override void Init()
    {
        base.Init ();  
    }

	public override void LocationConnect(AssignableLocation location)
	{
		base.LocationConnect(location);

		_routeToNest = NetworkManager.instance.GetRouteToNest(location);

	}

	protected override void HandleAssignedWorker(WorkerAnt assignedAnt)
	{
		assignedAnt.Assign(_routeToNest);

	}
}
