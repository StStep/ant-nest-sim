using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A class acting as an ordered list of <see cref="Node"/> objects.
/// </summary>
public class Path :IEnumerable<Node>
{
	/// The list of nodes wrapped by this class
	private List<Node> _nodelist;

	/// <summary>
	/// Initializes a new instance of the <see cref="Path"/> class.
	/// </summary>
	public Path()
	{
		_nodelist = new List<Node>();
	}

	/// <summary>
	/// Gets or sets the <see cref="Path"/> at the specified index.
	/// </summary>
	/// <param name="index">Index to use</param>
	public Node this[int index]  
	{  
		get { return _nodelist[index]; }  
		set { _nodelist.Insert(index, value); }  
	}

	/// <summary>
	/// Gets the count of the list.
	/// </summary>
	/// <value>The count.</value>
	public int Count
	{
		get
		{
			return _nodelist.Count;
		}
	}

	/// <summary>
	/// Adds the node to the path.
	/// </summary>
	/// <param name="node">Node to add</param>
	public void AddNode(Node node)
	{
		_nodelist.Add(node);
	}

	/// <summary>
	/// Gets the enumerator for iteration.
	/// </summary>
	/// <returns>The enumerator.</returns>
	public IEnumerator<Node> GetEnumerator()
	{
		return _nodelist.GetEnumerator();
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
	/// Reverse this path.
	/// </summary>
	public void Reverse()
	{
		_nodelist.Reverse();
	}

	/// <summary>
	/// This function returns a shallow copy of the Path
	/// </summary>
	/// <returns>A shallow copy of the path</returns>
	public Path Clone()
	{
		Path newPath = new Path();
		foreach(Node node in _nodelist)
		{
			newPath.AddNode(node);
		}

		return newPath;
	}
}
