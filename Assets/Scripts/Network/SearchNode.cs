using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// This class is used to wrap a <see cref="Location"/>  object for
/// A* network searching.
/// </summary>
public class SearchNode : IComparable<SearchNode> , IEquatable<Location> 
{

	/// <summary>
	/// The wrapped location
	/// </summary>
	public Location location;

	/// <summary>
    /// The parent <see cref="SearchNode"/> object for ordering
	/// </summary>
	public SearchNode parentSearchNode;

	/// <summary>
	/// G, the calculated cost to the node
	/// </summary>
	public float costToNode;

	/// <summary>
	/// H, heuristic cost, the estimated cost distance to the goal, from this node
	/// </summary>
	public float estCostToGoal;

	/// <summary>
	/// Gets F, the total cost of this node
	/// </summary>
	/// <value>The total cost of this node, the calcualted cost to the node plus the estimated cost
	/// to the goal from this node.</value>
	public float F
	{
		get
		{
			return (costToNode + estCostToGoal);
		}
	}

	/// <summary>
    /// Initializes a new instance of the <see cref="SearchNode"/> class.
	/// </summary>
    /// <param name="location">The location to wrap.</param>
    /// <param name="parentSearchNode">The parent of this search node.</param>
	/// <param name="costToNode">The calculated cost to this node.</param>
	/// <param name="estCostToGoal">The estimated cost to goal from this node.</param>
	public SearchNode(Location location, SearchNode parentSearchNode, float costToNode, float estCostToGoal )
	{
		this.location = location;
		this.parentSearchNode = parentSearchNode;
		this.costToNode = costToNode;
		this.estCostToGoal = estCostToGoal;
	}

	/// <summary>
	/// Compares the search node's total cost to a given search node's total cost.
	/// </summary>
	/// <returns>
	/// The return is 0 if their values are equal, -1 if the instance
	/// is greater than the value, and 1 if the value is greater than the instance
	/// </returns>
    /// <param name="otherSearchNode">Other search node.</param>
	/// <para>
	/// This is defined for the IComparable interface
	/// </para>
	public int CompareTo(SearchNode otherSearchNode) {
		if (otherSearchNode == null) 
			return 1;

		return this.F.CompareTo(otherSearchNode.F);

	}

	/// <summary>
    /// Determines whether the specified <see cref="Location"/> is equal to the current <see cref="SearchNode"/>.
	/// </summary>
    /// <param name="location">The <see cref="Location"/> to compare with the current <see cref="SearchNode"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Location"/> is equal to the current <see cref="SearchNode"/>; otherwise, <c>false</c>.</returns>
	public bool Equals(Location location)
	{
		if (location == null) return false;
		return (this.location.LocID.Equals(location.LocID));
	}
}
