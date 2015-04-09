using UnityEngine;
using System.Collections;

public class WorkerAnt : Ant {
	
	public override void Init()
	{
		base.Init();

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// This function starts the streaming order for the ant along the given paths
	/// </summary>
	/// <returns><c>true</c>, if to stream was ordered, <c>false</c> otherwise.</returns>
	/// <param name="orderName">The name of the order.</param>
	/// <param name="toDest">The path that is from the nest to the destination location.</param>
	/// <param name="toNest">The path that is from the destination location to the nest.</param>
	public bool OrderToStream(string orderName, Path toDest, Path toNest)
	{
		bool success = false;

		// Starting the order coroutine if not under order, it is idle
		if(IsIdle)
		{
			currentOrder = orderName;
			StartCoroutine(OrdStream(toDest, toNest));
			success = true;
		}

		return success;
	}

	#region Coroutine Actions

	/// <summary>
	/// The order coroutine that has the ant continuously walk back and forth the given paths.
	/// </summary>
	/// <returns>Returns IEnumerator, intended to be used as a coroutine.</returns>
	/// <param name="toDest">The path that is from the nest to the destination location.</param>
	/// <param name="toNest">The path that is from the destination location to the nest.</param>
	/// <para>
	/// This coroutine has the ant moving along each path, starting from the nest,
	/// going to the destination location, and then turning around to go
	/// back to the nest. This order can be naturally finished by setting the ant's
	/// currentlyAssigned variable to false. In that instance, the ant will end 
	/// the order at the nest.
	/// </para>
	protected IEnumerator OrdStream(Path toDest, Path toNest)
	{

		// TODO this is temporary to create visual nice-ness and to avoid clumping
		// Later build a delay in to processing, arriving at each location
		float waitTime = Random.Range(0.1f, 1f);

		ExitNest();

		// Repeat movement until no longer assigned to order
		while(assigned)
		{
			yield return new WaitForSeconds(waitTime);

			// Move from the nest to the destination location
			foreach(Node node in toDest)
			{
				// Move to node
				yield return StartCoroutine(ActMoveToPosition(node.Position));
			}

			yield return new WaitForSeconds(waitTime);

			// Move from the destination location to the nest
			foreach(Node node in toNest)
			{
				// Move to node
				yield return StartCoroutine(ActMoveToPosition(node.Position));
			}
		}

		// Go idle and enter nest
		currentOrder = "";
		NetworkManager.instance.nest.GiveWorkerAntSelf(this);
		EnterNest();
	}

	#endregion
}
