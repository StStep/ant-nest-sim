using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class stores a network of nodes and allows for searching the network for paths
/// </summary>
/// <para>
/// This class can save a network of objects of the <see cref="Node"/> class, and can 
/// search through the network, finding and returning a <see cref="Path"/> using A* search method.
/// After the network is created nodes are added with calls to ConnectNodes() which handles 
/// the inter-node connections necessary for traversing the network. The function GetPath() returns a Path
/// object if a path can be found between the two provided nodes.
/// </para>
/// <remarks>
/// Any paths provided by this class become invalid if the network changes.
/// Therefore, any network changes should be handled carefully, with new path requests
/// being frequent.
/// </remarks>
public class Network {
	  
	// TODO Store a cache of past path searches, clearing the path list when the network changes
	// As a way of avoiding doing A* searches for the same destination/start or the compliment

	/// A dictionary of nodes in the network, using the location ID of the nodes 
	protected Dictionary<int, Node> _netNodes;

	/// <summary>
	/// Initializes a new instance of the <see cref="Network"/> class around a primary <see cref="Node"/>.
	/// </summary>
	/// <param name="primaryNode">The node to center the network around.</param>
	public Network() {
		_netNodes = new Dictionary<int, Node>();
	}

	// This function connects all of the child nodes 
	// to the parent node
	/// <summary>
	/// This function first adds the nodes to the nework, and then connects the children nodes to the parent.
	/// </summary>
	/// <param name="parentNode">The parent node, to which each child is connected.</param>
	/// <param name="childrenNodes">The children node to connect to the parent node.</param>
	public void ConnectNodes(Node parentNode, params Node[] childrenNodes)
	{
		_netNodes[parentNode.LocID] = parentNode;

		foreach(Node child in childrenNodes)
		{
			_netNodes[child.LocID] = child;
			parentNode.AddConnection(child);
			child.AddConnection(parentNode);
		}
	}

	/// <summary>
	/// Calculates the distance squared.
	/// </summary>
	/// <returns>The square distance between parameter nodes.</returns>
	/// <param name="node1">First node used in distance calculation.</param>
	/// <param name="node2">Second node used in distance calculation.</param>
	/// <para>
	/// This function returns the square distance between two nodes. This is intended
	/// for distance comparison, where the exact absolute distance is not important. 
	/// </para>
	private static float calculateDistance(Node node1, Node node2)
	{
		return 	((node1.Position.x - node2.Position.x) * (node1.Position.x - node2.Position.x) ) + 
				((node1.Position.y - node2.Position.y) * (node1.Position.y - node2.Position.y) );
	}

	/// <summary>
	/// This function returns an object of the class <see cref="Path"/> if 
	/// a path exists bewteen the provided nodes. Otherwise, NULL is returned.
	/// </summary>
	/// <returns>A <see cref="Path"/> object describing the shortest path from the start to end node</returns>
	/// <param name="startNode">The start node in the desired path.</param>
	/// <param name="endNode">The destination node of the desired path.</param>
	/// <para>
	/// This function uses and A* search to find the shortest path between two nodes
	/// on the network.
	/// </para>
	public Path GetPath(Node startNode, Node endNode)
	{
		if(startNode == null || endNode == null)
		{
			return null;
		}

		// Already at node
		Path retPath;
		if(startNode.LocID == endNode.LocID)
		{
			retPath = new Path();
			return retPath;
		}

		Node start;
		Node goal;

		_netNodes.TryGetValue(startNode.LocID, out start);//_netNodes[startNode.LocID];
		_netNodes.TryGetValue(endNode.LocID, out goal);//_netNodes[endNode.LocID];
		if(start == null || goal == null)
		{
			return null;
		}

		// Make Lists for search
		List<NodeWrap> openList = new List<NodeWrap>();
		List<NodeWrap> closedList = new List<NodeWrap>();

		// Place starting node in open list list
		openList.Add(new NodeWrap(start, null, 0, calculateDistance(start, goal)));

		NodeWrap curNodeWrap;
		NodeWrap goalNodeWrap = null;

		while(true)
		{
			//Exit upon empty openList
			if(openList.Count == 0)
			{
				break;
			}

			// Sort openList and choose lowest F
			openList.Sort();

			// Take lowest F in openList and move to closedList
			curNodeWrap = openList[0];
			openList.RemoveAt(0);
			closedList.Add(curNodeWrap);

			// Check for goal node
			if(curNodeWrap.Equals(goal))
			{
				goalNodeWrap = curNodeWrap;
				break;
			}


			// Add connected nodes to open list if not already listed in closedList, update if on openList
			foreach(Node node in curNodeWrap.node)
			{
				bool found = false;

				// Ignore node if on closed list
				foreach(NodeWrap nodeWrap in closedList)
				{
					if(nodeWrap.Equals(node))
					{
						found = true;
						break;
					}
				}

				if(found)
				{
					continue;
				}

				// Potentially update NodeWrap if it is on the openList and the costToNode is less
				foreach(NodeWrap nodeWrap in openList)
				{
					if(nodeWrap.Equals(node))
					{
						// Compare costToNode, update parent if node has lower cost
						float newCostToNode = curNodeWrap.costToNode + calculateDistance(curNodeWrap.node, node);
						if(newCostToNode < nodeWrap.costToNode)
						{
							nodeWrap.parentNodeWrap = curNodeWrap;
							nodeWrap.costToNode = newCostToNode;
						}

						found = true;
						break;
					}
				}
				
				if(found)
				{
					continue;
				}

				openList.Add(new NodeWrap(node, curNodeWrap, curNodeWrap.costToNode + calculateDistance(curNodeWrap.node, node), calculateDistance(node, goal)));
			}
		}

		if(goalNodeWrap == null)
		{
			return null;
		}

		// Make a path if the final NodeWrap, a path from start to goal, was found
		retPath = new Path();
		NodeWrap parent = goalNodeWrap;

		// Create path from goal to start
		while(parent != null)
		{
			retPath.AddNode(parent.node);
			parent = parent.parentNodeWrap;
		}

		// Reverse path becuase it was created from goal to start
		retPath.Reverse();

		return retPath;
	}
}
