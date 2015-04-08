using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class is for a node in the <see cref="Network"/> object
/// </summary>
public class Node :IEnumerable<Node> , IEquatable<Node>
{
	/// The location this node represents
	protected Location _location;
	/// <summary>
	/// Gets the location the node represents..
	/// </summary>
	/// <value>The location.</value>
	public Location Location
	{
		get
		{
			return _location;
		}
	}
	
	/// The location ID, unique for each <see cref="Node"/> object
	protected int _locID;
	/// <summary>
	/// The location ID, unique for each <see cref="Node"/> object
	/// </summary>
	/// <value>The location I.</value>
	public int LocID 
	{
		get
		{ 
			return _locID; 
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
			return _location.transform.position;
		}
	}
	
	/// The list of adjacent nodes in the network.
	protected List<Node> _connectedNodes;

	/// <summary>
	/// Initializes a new instance of the <see cref="Node"/> class for a given location
	/// </summary>
	/// <param name="location">The NodeLocation this node represents.</param>
	public Node(Location location) 
	{
		_location = location;
		_connectedNodes = new List<Node>();
		_locID = NetworkManager.GetNextLocID();
	}

	/// <summary>
	/// Adds the given node to the connection list.
	/// </summary>
	/// <param name="node">THe node to add to connection list.</param>
	public void AddConnection(Node node)
	{
		if(!_connectedNodes.Contains(node))
		{
			_connectedNodes.Add(node);
		}
	}

	/// <summary>
	/// Removes the given node from the connection list.
	/// </summary>
	/// <param name="node">Node.</param>
	public void RemoveConnection(Node node)
	{
		if(_connectedNodes.Contains(node))
		{
			_connectedNodes.Remove(node);
		}
	}

	/// <summary>
	/// Gets or sets the <see cref="Node"/> at the specified index.
	/// </summary>
	/// <param name="index">Index.</param>
	public Node this[int index]  
	{  
		get { return _connectedNodes[index]; }  
		set { _connectedNodes.Insert(index, value); }  
	} 

	/// <summary>
	/// Gets the enumerator for iteration.
	/// </summary>
	/// <returns>The enumerator.</returns>
	public IEnumerator<Node> GetEnumerator()
	{
		return _connectedNodes.GetEnumerator();
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
	/// Determines whether the specified <see cref="Node"/> is equal to the current <see cref="Node"/>.
	/// </summary>
	/// <param name="node">The <see cref="Node"/> to compare with the current <see cref="Node"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="Node"/> is equal to the current <see cref="Node"/>; otherwise, <c>false</c>.</returns>
	public bool Equals(Node node)
	{
		if (node == null) return false;
		return (this.LocID.Equals(node.LocID));
	}
}
