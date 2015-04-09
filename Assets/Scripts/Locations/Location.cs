using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This is the abstract class for game locations. Locations manage the ants assigned to them.
/// </summary>
public abstract class Location : MonoBehaviour {

	/// <summary>
	/// The text object displayed above the location
	/// </summary>
	public Text locationText;

	/// <summary>
	/// The node object representing this location in the network.
	/// </summary>
	[HideInInspector]public Node networkNode;

	/// <summary>
	/// Gets the location's ID
	/// </summary>
	/// <value>The location ID for the <see cref="Node"/> object representing this location in the network.</value>
	public int LocationID
	{
		get
		{
			return networkNode.LocID;
		}
	}

	/// <summary>
	/// The potential click types on the gameobject.
	/// </summary>
	protected enum ClickType { NoClick, LeftClick, RightClick }

	/// <summary>
	/// The most recent click type on the gameobject
	/// </summary>
	protected ClickType clickStatus;

	/// <summary>
	/// This is a queue of worker ants assigned to this location
	/// </summary>
	protected Queue<WorkerAnt> workerAntQ;

	/// <summary>
	/// Gets the amount worker ants assigned to this location
	/// </summary>
	/// <value>The count of the worker ant queue.</value>
	public int WorkerAntCount
	{
		get
		{
			return workerAntQ.Count;
		}
	}

	/// <summary>
	/// This initializes this instance, for intializaing internal objects 
	/// before Awake() is called.
	/// </summary>
	public virtual void Init()
	{
		clickStatus = ClickType.NoClick;
		networkNode = new Node(this);
		workerAntQ = new Queue<WorkerAnt>();

		locationText.text = WorkerAntCount + " Workers";
	}

	protected virtual void Awake()
	{
		// If init was not called for this object yet, call it
		if(networkNode == null)
		{
			Init();
		}
	}
	
	protected virtual void Start () 
	{

	}

	protected virtual void Update () 
	{
	
	}

	/// <summary>
	/// This function should be called when a mouse clicks
	/// down on the game object
	/// </summary>
	public virtual void ClickDown()
	{
		if (Input.GetMouseButtonDown(0))
		{
			clickStatus = ClickType.LeftClick;
		}
		else if (Input.GetMouseButtonDown(1))
		{
			clickStatus = ClickType.RightClick;
		}
		else
		{
			clickStatus = ClickType.NoClick;
		}
	}

	/// <summary>
	/// This function cancels a click on the gameobject, should
	/// be called when an mouse curser leaves the gameobject.
	/// </summary>
	public virtual void ClickCancel()
	{
		clickStatus = ClickType.NoClick;
	}

	/// <summary>
	/// This function should be called when a mouse clicks
	/// up after clicking on the game object.
	/// </summary>
	public virtual void ClickUp()
	{
		if (clickStatus == ClickType.LeftClick)
		{
			Debug.Log("You left clicked on a node");
		}
		else if (clickStatus == ClickType.RightClick)
		{
			Debug.Log("You right clicked on a node");
		}
	}
}
