using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// This class is used to wrap a <see cref="Node"/>  object for
/// A* network searching.
/// </summary>
public class NodeWrap : IComparable<NodeWrap> , IEquatable<Node> 
{

	/// <summary>
	/// The wrapped node
	/// </summary>
	public Node node;

	/// <summary>
	/// The parent <see cref="NodeWarp"/> object for ordering
	/// </summary>
	public NodeWrap parentNodeWrap;

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
	/// Initializes a new instance of the <see cref="NodeWrap"/> class.
	/// </summary>
	/// <param name="node">The node to wrap.</param>
	/// <param name="parentNodeWrap">The parent of this node wrap.</param>
	/// <param name="costToNode">The calculated cost to this node.</param>
	/// <param name="estCostToGoal">The estimated cost to goal from this node.</param>
	public NodeWrap(Node node, NodeWrap parentNodeWrap, float costToNode, float estCostToGoal )
	{
		this.node = node;
		this.parentNodeWrap = parentNodeWrap;
		this.costToNode = costToNode;
		this.estCostToGoal = estCostToGoal;
	}

	/// <summary>
	/// Compares the node wrap total cost to a given node wrap's total cost.
	/// </summary>
	/// <returns>
	/// The return is 0 if their values are equal, -1 if the instance
	/// is greater than the value, and 1 if the value is greater than the instance
	/// </returns>
	/// <param name="otherNodeWrap">Other node wrap.</param>
	/// <para>
	/// This is defined for the IComparable interface
	/// </para>
	public int CompareTo(NodeWrap otherNodeWrap) {
		if (otherNodeWrap == null) 
			return 1;

		return this.F.CompareTo(otherNodeWrap.F);

	}

	/// <summary>
	/// Determines whether the specified <see cref="Node"/> is equal to the current <see cref="NodeWrap"/>.
	/// </summary>
	/// <param name="node">The <see cref="Node"/> to compare with the current <see cref="NodeWrap"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="Node"/> is equal to the current <see cref="NodeWrap"/>; otherwise, <c>false</c>.</returns>
	public bool Equals(Node node)
	{
		if (node == null) return false;
		return (this.node.LocID.Equals(node.LocID));
	}
}
