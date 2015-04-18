using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This class is for a location in the <see cref="Network"/> object
/// </summary>
public abstract class Location :MonoBehaviour, IEnumerable<Location> , IEquatable<Location>
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
    /// Gets the amount worker ants assigned to this location
    /// </summary>
    /// <value>The count of the worker ant queue.</value>
    public int WorkerAntCount
    {
        get
        {
            return workerAntList.Count;
        }
    }

    /// <summary>
    ///  The game position of the node.
    /// </summary>
    /// <value>The position.</value>
    public Vector2 Position
    {
        get
        {
            return transform.position;
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
    /// This is a list of worker ants assigned to this location. Ants are inserted
    /// at the beggening and removed from the end.
    /// </summary>
    protected List<WorkerAnt> workerAntList;

	/// The location ID, unique for each <see cref="Node"/> object
	protected int _locID;
	/// <summary>
	/// The location ID, unique for each <see cref="Node"/> object
	/// </summary>
	/// <value>The location ID.</value>
	public int LocID 
	{
		get
		{ 
			return _locID; 
		}
	}
	
	/// The list of adjacent location in the network.
	protected List<Location> _connectedLocations;

    /// The paths to adjacent location in network
    protected Dictionary<int, Path> _connectedPaths;

    /// <summary>
    /// This initializes this instance, for intializaing internal objects 
    /// before Awake() is called.
    /// </summary>
    public virtual void Init()
    {
        clickStatus = ClickType.NoClick;
        workerAntList = new List<WorkerAnt>();

        _connectedLocations = new List<Location>();
        _connectedPaths = new Dictionary<int, Path>();
        _locID = NetworkManager.GetNextLocID();
        
        upperText.text = WorkerAntCount + " Workers";
    }
    
    protected virtual void Awake()
    {
        // If init was not called for this object yet, call it
        if(workerAntList == null)
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
    /// Adds the given location and path to the connection list.
	/// </summary>
    /// <param name="location">The location to add to connection list.</param>
    /// <param name="pathToLocation">Path to the location to add to the path list.</param>
	public void AddConnection(Location location, Path pathToLocation)
	{
		if(!_connectedLocations.Contains(location))
		{
			_connectedLocations.Add(location);
            _connectedPaths.Add(location.LocID, pathToLocation);
		}
	}

	/// <summary>
    /// Removes the given location from the connection list.
	/// </summary>
    /// <param name="location">The location to remove.</param>
	public void RemoveConnection(Location location)
	{
		if(_connectedLocations.Contains(location))
		{
			_connectedLocations.Remove(location);
            _connectedPaths.Remove(location.LocID);
		}
	}

	/// <summary>
    /// Gets or sets the connected <see cref="Location"/> at the specified index.
	/// </summary>
	/// <param name="index">Index.</param>
	public Location this[int index]  
	{  
		get { return _connectedLocations[index]; }  
		set { ; }  
	} 

    /// <summary>
    /// Gets a paths to connected location.
    /// </summary>
    /// <returns>The path to the given location, or null if one doesn't exist.</returns>
    /// <param name="location">The location to look for a path to.</param>
    public Path pathToLocation(Location location) 
    {
        if(_connectedLocations.Contains(location))
        {
            return _connectedPaths[location.LocID];
        }
        else
        {
            return null;
        }
    }

	/// <summary>
	/// Gets the enumerator for iteration.
	/// </summary>
	/// <returns>The enumerator.</returns>
	public IEnumerator<Location> GetEnumerator()
	{
		return _connectedLocations.GetEnumerator();
	}

	/// <summary>
	/// Gets the enumerator for the IEnumerable interface
	/// </summary>
	/// <returns>The enumerator.</returns>
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	/// <summary>
    /// Determines whether the specified <see cref="Location"/> is equal to the current <see cref="Location"/>.
	/// </summary>
    /// <param name="location">The <see cref="Location"/> to compare with the current <see cref="Location"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Location"/> is equal to the current <see cref="Location"/>; otherwise, <c>false</c>.</returns>
	public bool Equals(Location location)
	{
		if (location == null) return false;
		return (this.LocID.Equals(location.LocID));
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
            Debug.Log("You left clicked on a location");
        }
        else if (clickStatus == ClickType.RightClick)
        {
            Debug.Log("You right clicked on a location");
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
    /// This function removes a specific ant from a location list.
    /// </summary>
    /// <param name="antToRemove">The ant to remove.</param>
    public void RemoveAnt(Ant antToRemove)
    {
        if(antToRemove is WorkerAnt)
        {
            workerAntList.Remove((WorkerAnt)antToRemove);
            upperText.text = WorkerAntCount + " Workers";
        }
        else
        {
            Debug.Log("RemoveAnt: ERROR - Unknwin ant class");
        }
    }
}
