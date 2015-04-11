using UnityEngine;
using System.Collections;

public class WorkerAnt : Ant {
	
	public override void Init()
	{
		base.Init();

	}

	// Use this for initialization
	protected override void Start() {
		base.Start();
	
	}
	
	// Update is called once per frame
	protected override void  Update() {
		base.Update();
	}

	/// <summary>
	/// This function starts the streaming order for the ant along the given paths
	/// </summary>
	/// <returns><c>true</c>, if to stream was ordered, <c>false</c> otherwise.</returns>
	/// <param name="orderName">The name of the order.</param>
	/// <param name="pathToDest">The path that is from the start location to the destination location.</param>
	/// <param name="returnPath">The path that is from the destination location to the start location.</param>
	public bool OrderToStream(string orderName, Path pathToDest, Path returnPath)
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
	protected IEnumerator OrdStream(Path pathToDest, Path returnPath)
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
		yield return StartCoroutine(pathToDest[0].Location.Exit(this));

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
				yield return StartCoroutine(pathToDest[i].Location.Visit(this));

				// If the ant is full after any node or needs food, return to origin, starting from current node
				if(this.IsFull || findingFood)
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
				yield return StartCoroutine(returnPath[i].Location.Visit(this));
			}
		}
		// Go idle having finished movement
		currentOrder = "";

		// Enter the starting node
		yield return StartCoroutine(pathToDest[0].Location.Enter(this));
	}

	#endregion
}
