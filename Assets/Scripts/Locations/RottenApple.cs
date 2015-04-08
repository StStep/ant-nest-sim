using UnityEngine;
using System.Collections;


// TODO make a higher upp class for worker assignable nodes

/// <summary>
/// This calls represents the Nest game location
/// </summary>
public class RottenApple : Location {

	/// <summary>
	/// The amount of worker ants in the nest
	/// </summary>
	protected int workerAntCount;

	/// <summary>
	/// The amount of worker ants added or removed with each click
	/// </summary>
	protected const int WorkerAddNumber = 10;

	protected Path pathToNest;

	/// <summary>
	/// This initializes this instance, for intializaing internal objects 
	/// before Awake() is called.
	/// </summary>
	public override void Init()
	{
		base.Init();
		
		workerAntCount = 0;
		locationText.text = workerAntCount + " Workers";
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
		// TODO clean this up for actual usage
		if(pathToNest == null)
		{
			Network tempNetwork = NetworkManager.instance.LocationNetwork;
			pathToNest = tempNetwork.GetPath(tempNetwork.PrimaryNode, networkNode);
		}

		if (clickStatus == ClickType.LeftClick)
		{
			if(pathToNest != null)
			{
				Debug.Log("You added workers to an apple");
				workerAntCount += NetworkManager.instance.nest.TakeWorkers(WorkerAddNumber);
				locationText.text = workerAntCount + " Workers";
			}
		}
		else if (clickStatus == ClickType.RightClick)
		{
			if(pathToNest != null)
			{
				Debug.Log("You took away workers from an apple");
				if(workerAntCount >= WorkerAddNumber) {
					workerAntCount -= NetworkManager.instance.nest.GiveWorkers(WorkerAddNumber);
				}
				else 
				{
					workerAntCount -= NetworkManager.instance.nest.GiveWorkers(workerAntCount);
				}
				locationText.text = workerAntCount + " Workers";
			}
		}
	}
}
