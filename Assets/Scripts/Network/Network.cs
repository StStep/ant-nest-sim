using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class stores a network of locations and allows for searching the network for routes.
/// </summary>
/// <para>
/// This class can save a network of objects of the <see cref="Location"/> class, and can 
/// search through the network, finding and returning a <see cref="Route"/> using A* search method.
/// After the network is created locations are added with calls to ConnectLocations() which handles 
/// the inter-node connections necessary for traversing the network. The function GetRoute() returns a Route
/// object if a route can be found between the two provided locations.
/// </para>
/// <remarks>
/// Any routes provided by this class become invalid if the network changes.
/// Therefore, any network changes should be handled carefully, with new route requests
/// being frequent.
/// </remarks>
public class Network {
	  
	// TODO Store a cache of past route searches, clearing the route list when the network changes
	// As a way of avoiding doing A* searches for the same destination/start or the compliment

	/// A dictionary of locaitons in the network, using the location ID of the nodes 
	protected Dictionary<int, Location> _netLocs;

	/// <summary>
	/// Initializes a new instance of the <see cref="Network"/>.
	/// </summary>
	public Network() {
		_netLocs = new Dictionary<int, Location>();
	}

	/// <summary>
	/// This function first adds the locations together and connects them with the given path.
	/// </summary>
    /// <param name="locationA">Location A of the added path.</param>
    /// <param name="locationB">Location B of the added path.</param>
    /// <param name="pathBetween">The path to use to connect the two given locations.</param>
    public void ConnectLocations(Location locationA, Location locationB, Path pathBetween)
	{
		_netLocs[locationA.LocID] = locationA;
		_netLocs[locationB.LocID] = locationB;

        pathBetween.ConnectLocations(locationA, locationB);
	}

	/// <summary>
	/// Calculates the distance squared.
	/// </summary>
    /// <returns>The square distance between the parameter location.</returns>
    /// <param name="locationA">First location used in distance calculation.</param>
    /// <param name="locationB">Second location used in distance calculation.</param>
	/// <para>
    /// This function returns the square distance between two locations. This is intended
	/// for distance comparison, where the exact absolute distance is not important. 
	/// </para>
	private static float calculateDistance(Location locationA, Location locationB)
	{
		return 	((locationA.Position.x - locationB.Position.x) * (locationA.Position.x - locationB.Position.x) ) + 
				((locationA.Position.y - locationB.Position.y) * (locationA.Position.y - locationB.Position.y) );
	}

	/// <summary>
    /// This function returns an object of the class <see cref="Route"/> if 
	/// a route exists bewteen the provided locations. Otherwise, NULL is returned.
	/// </summary>
    /// <returns>A <see cref="Route"/> An object describing the shortest path from the start to end location</returns>
    /// <param name="startLoc">The start location in the desired route.</param>
    /// <param name="endLoc">The destination location of the desired route.</param>
	/// <para>
    /// This function uses and A* search to find the shortest route between the two given locations
	/// on the network.
	/// </para>
	public Route GetRoute(Location startLoc, Location endLoc)
	{
		if(startLoc == null || endLoc == null)
		{
			return null;
		}

		// Already at the location
		Route retRoute;
		if(startLoc.LocID == endLoc.LocID)
		{
			retRoute = new Route();
			return retRoute;
		}

		Location start;
		Location goal;

		_netLocs.TryGetValue(startLoc.LocID, out start);
		_netLocs.TryGetValue(endLoc.LocID, out goal);
		if(start == null || goal == null)
		{
			return null;
		}

		// Make Lists for search
		List<SearchNode> openList = new List<SearchNode>();
		List<SearchNode> closedList = new List<SearchNode>();

		// Place starting node in open list list
		openList.Add(new SearchNode(start, null, 0, calculateDistance(start, goal)));

		SearchNode curSearchNode;
		SearchNode goalSearchNode = null;

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
			curSearchNode = openList[0];
			openList.RemoveAt(0);
			closedList.Add(curSearchNode);

			// Check for goal node
			if(curSearchNode.Equals(goal))
			{
				goalSearchNode = curSearchNode;
				break;
			}


			// Add connected nodes to open list if not already listed in closedList, update if on openList
			foreach(Location location in curSearchNode.location)
			{
				bool found = false;

				// Ignore node if on closed list
				foreach(SearchNode searchNode in closedList)
				{
					if(searchNode.Equals(location))
					{
						found = true;
						break;
					}
				}

				if(found)
				{
					continue;
				}

                // Potentially update SearchNode if it is on the openList and the costToNode is less
				foreach(SearchNode searchNode in openList)
				{
					if(searchNode.Equals(location))
					{
						// Compare costToNode, update parent if node has lower cost
						float newCostToNode = curSearchNode.costToNode + calculateDistance(curSearchNode.location, location);
						if(newCostToNode < searchNode.costToNode)
						{
							searchNode.parentSearchNode = curSearchNode;
							searchNode.costToNode = newCostToNode;
						}

						found = true;
						break;
					}
				}
				
				if(found)
				{
					continue;
				}

				openList.Add(new SearchNode(location, curSearchNode, curSearchNode.costToNode + calculateDistance(curSearchNode.location, location), calculateDistance(location, goal)));
			}
		}

		if(goalSearchNode == null)
		{
			return null;
		}

        // Make a route if the final NodeWrap, a route from start to goal, was found
		retRoute = new Route();
		SearchNode parent = goalSearchNode;

		// Create route from goal to start
		while(parent != null)
		{
			retRoute.AddLocation(parent.location);
			parent = parent.parentSearchNode;
		}

        // Reverse route becuase it was created from goal to start
		retRoute.Reverse();

		return retRoute;
	}
}
