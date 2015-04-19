using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A class acting as an ordered list of <see cref="Location"/> objects.
/// </summary>
public class Route :IEnumerable<Location>
{
    // TODO How about make it reusable? Maybe Assignments feed there path into the network to rebuild?

	private bool locked;

	/// The list of locations wrapped by this class
	private List<Location> _locList;

	/// <summary>
    /// Initializes a new instance of the <see cref="Route"/> class.
	/// </summary>
	public Route()
	{
		_locList = new List<Location>();
		locked = false;
	}

	/// <summary>
    /// Gets or sets the <see cref="Location"/> at the specified index.
	/// </summary>
	/// <param name="index">Index to use</param>
	public Location this[int index]  
	{  
		get 
		{ 
			return _locList[index]; 
		}  
		set 
		{ 
			if(!locked) 
			{
				_locList.Insert(index, value); 
			}
		}  
	}

	/// <summary>
	/// Gets the count of the list.
	/// </summary>
	/// <value>The count.</value>
	public int Count
	{
		get
		{
			return _locList.Count;
		}
	}

	/// <summary>
    /// Adds the location to the route.
	/// </summary>
    /// <param name="location">Location to add</param>
	public void AddLocation(Location location)
	{
		if(!locked) 
		{
			_locList.Add(location);
		}
	}

	/// <summary>
	/// Gets the enumerator for iteration.
	/// </summary>
	/// <returns>The enumerator.</returns>
	public IEnumerator<Location> GetEnumerator()
	{
		return _locList.GetEnumerator();
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
	/// Reverse this route.
	/// </summary>
	public void Reverse()
	{
		if(!locked) 
		{
			_locList.Reverse();
		}
	}

	/// <summary>
	/// Lock this instance.
	/// </summary>
	public void Lock()
	{
		locked = true;
	}
	
/*	/// <summary>
    /// This function returns a shallow copy of the Route
	/// </summary>
    /// <returns>A shallow copy of the Route</returns>
	public Route Clone()
	{
		Route newRoute = new Route();
		foreach(Location location in _locList)
		{
			newRoute.AddLocation(location);
		}

		return newRoute;
	}*/
}
