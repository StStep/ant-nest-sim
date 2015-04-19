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

        _connectedLocations = new List<Location>();
        _connectedPaths = new Dictionary<int, Path>();
        _locID = NetworkManager.GetNextLocID();
        
        upperText.text = "";
        lowerText.text = "";
    }
    
    protected virtual void Awake()
    {
        // If init was not called for this object yet, call it
        if(_locID == 0)
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

	public virtual void Enter(Ant ant)
	{
		Exit(ant);
	}

	public virtual void Exit(Ant ant)
	{
		ant.HandleLocationExit();
	}

	public virtual void TakePathTo(Ant ant, Location destLocation)
	{
		_connectedPaths[destLocation.LocID].TakePathFrom(ant, this);
	}
}
