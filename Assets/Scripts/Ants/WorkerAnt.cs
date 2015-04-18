using UnityEngine;
using System.Collections;

/// <summary>
/// This is the ant that collects food for the nest.
/// </summary>
public class WorkerAnt : Ant {

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

	/// <summary>
	/// This function starts the streaming order for the ant along the given paths
	/// </summary>
	/// <returns><c>true</c>, if to stream was ordered, <c>false</c> otherwise.</returns>
	/// <param name="orderName">The name of the order.</param>
	/// <param name="pathToDest">The path that is from the start location to the destination location.</param>
	/// <param name="returnPath">The path that is from the destination location to the start location.</param>
	public bool OrderToStream(string orderName, Route pathToDest, Route returnPath)
	{
		bool success = false;

		// Starting the order coroutine if not under order, it is idle
		if(IsIdle)
		{
			currentOrder = orderName;
			StartCoroutine(OrdStream(pathToDest, returnPath));
			success = true;
		}

		return success;
	}

	#region Coroutine Actions

	/// <summary>
	/// The order coroutine that has the ant continuously walk back and forth the given paths.
	/// </summary>
	/// <returns>Returns IEnumerator, intended to be used as a coroutine.</returns>
	/// <param name="pathToDest">The path that is from the start location to the destination location.</param>
	/// <param name="returnPath">The path that is from the destination location to the start location.</param>
	/// <para>
	/// This coroutine has the ant moving along each path, starting from the start location,
	/// going to the destination location, and then turning around to go
	/// back to the start location. The starting location is exited at the before the 
	/// movement loop, and entered at the after the loop ends. Each node in between is visited.
	/// </para>
	protected IEnumerator OrdStream(Route pathToDest, Route returnPath)
	{
		// Check pathing reqiurements
		if((pathToDest[0] != returnPath[returnPath.Count - 1]) ||
		   (pathToDest[pathToDest.Count - 1] != returnPath[0]) ||
		   (pathToDest.Count != returnPath.Count))
		{
			Debug.Log("OrdStream: ERROR - given paths are not symmetrical");
			yield break;
		}

		// Exit the starting node
		yield return StartCoroutine(pathToDest[0].Exit(this));

		// Repeat movement until no longer assigned to order
		while(assigned)
		{
			// Move from the nest to the destination location
			int breakNode = 1;
			for(int i = 1; i < pathToDest.Count; i++)
			{
				// Move to node
				yield return StartCoroutine(ActMoveToPosition(pathToDest[i].Position));

				// Enter Node 
				yield return StartCoroutine(pathToDest[i].Visit(this));

				// If the ant is full after any node or needs food, return to origin, starting from current node
				if(this.IsFull || isStarving)
				{
					breakNode = returnPath.Count - i;
					break;
				}
			}

			// Move from the nest to the destination location, skipping the first path node, where the ant should be
			for(int i = breakNode; i < returnPath.Count; i++)
			{
				// Move to node
				yield return StartCoroutine(ActMoveToPosition(returnPath[i].Position));

				// Enter Node 
				yield return StartCoroutine(returnPath[i].Visit(this));
			}
		}
		// Go idle having finished movement
		currentOrder = "";

		// Enter the starting node
		yield return StartCoroutine(pathToDest[0].Enter(this));
	}

	#endregion
}
