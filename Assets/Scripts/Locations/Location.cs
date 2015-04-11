using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This is the abstract class for game locations. Locations manage the ants assigned to them.
/// </summary>
public abstract class Location : MonoBehaviour , IEquatable<Location>
{

	/// <summary>
	/// The text object displayed above the location
	/// </summary>
	public Text upperText;

	/// <summary>
	/// The text object displayed above the location
	/// </summary>
	public Text lowerText;

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

		upperText.text = WorkerAntCount + " Workers";
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

	/// <summary>
	/// This couroutine function allows an ant to visit a location. A visit consists of a
	/// consecutive enter cooroutine and exit cooroutine.
	/// </summary>
	/// <param name="visitingAnt">The ant that is visiting the location</param>
	public IEnumerator Visit(Ant visitingAnt)
	{
		yield return StartCoroutine(this.Enter(visitingAnt));
		yield return StartCoroutine(this.Exit(visitingAnt));
	}

	/// <summary>
	/// This couroutine function allows an ant to enter a location. This couroutine should be called 
	/// by the ant when the ant wants to enter a node.
	/// </summary>
	/// <param name="enteringAnt">The ant that is entering the location</param>
	public abstract IEnumerator Enter(Ant enteringAnt);

	/// <summary>
	/// This couroutine function allows an ant to exit a location. This couroutine should be called 
	/// by the ant when the ant wants to leave a node.
	/// </summary>
	/// <param name="exitingAnt">The ant that is exiting the location</param>
	public abstract IEnumerator Exit(Ant exitingAnt);

	/// <summary>
	/// Determines whether the specified <see cref="Location"/> is equal to the current <see cref="Location"/>.
	/// </summary>
	/// <param name="node">The <see cref="Location"/> to compare with the current <see cref="Location"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="Location"/> is equal to the current <see cref="Location"/>; otherwise, <c>false</c>.</returns>
	public bool Equals(Location location)
	{
		if (location == null) return false;
		return (this.networkNode.Equals(location.networkNode));
	}
}
