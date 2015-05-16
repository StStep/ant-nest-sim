using UnityEngine;
using System.Collections;

/// <summary>
/// This class is a signleton used for creating and providing an interface to the
/// location network.
/// </summary>
public class NetworkManager : MonoBehaviour {

	/// <summary>
	/// The single current instance of this class
	/// </summary>
	public static NetworkManager instance;

	/// <summary>
	/// The next location ID that will be assigned.
	/// </summary>
	private static int nextLocID = 0;

	/// <summary>
	/// This function request a unique location ID for finding
	/// specific locations in the network.
	/// </summary>
	/// <returns>The next location ID.</returns>
	public static int GetNextLocID() {
		return (nextLocID++);
	}

	/// <summary>
	/// The network of the locations.
	/// </summary>
	[HideInInspector]public Network LocationNetwork;

	/// <summary>
	/// The Nest, the primary location of the network.
	/// </summary>
	private Nest nest;

	void Awake()
	{
		// Only have one in game
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this) 
		{
			Destroy(gameObject);	
		}
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}

    void Start() 
    {
    }

	/// <summary>
	/// This function initializes and forms the location network.
	/// </summary>
	public Nest CreateNetwork()
	{
		this.nest = PrefabManager.instance.CreateNestObject(Vector2.zero);
		this.nest.transform.SetParent(transform);
		this.LocationNetwork = new Network();

		Vector2[] firstRing = new Vector2[5];

		firstRing[0].x = 6f;
		firstRing[0].y = 0f;

		firstRing[1].x = 0f;
		firstRing[1].y = 4f;

		firstRing[2].x = -4f;
		firstRing[2].y = 0f;

		firstRing[3].x = -3f;
		firstRing[3].y = -5f;

		firstRing[4].x = 3f;
		firstRing[4].y = -5f;
		
		// TODO Temp Create network first ring location, around nest
		Location[] tempLocs = new Location[firstRing.Length];
		for(int i = 0; i < firstRing.Length; i++)
		{
			tempLocs[i] = PrefabManager.instance.CreateEmptyLocationObject(firstRing[i]);
			tempLocs[i].transform.SetParent(transform);

            // Create network connections
			CreateNetworkPath(this.nest, tempLocs[i]);
        }

		// Connect ring together
		Location prevLoc = tempLocs[tempLocs.Length-1];
		foreach(Location loc in tempLocs)
		{
			// Create network connections
			CreateNetworkPath(prevLoc, loc);
			prevLoc = loc;
		}

		Vector2[] secondRing = new Vector2[4];

		secondRing[0].x = 8f;
		secondRing[0].y = 4f;
		
		secondRing[1].x = -6f;
		secondRing[1].y = 6f;
		
		secondRing[2].x = -6f;
		secondRing[2].y = -4f;
		
		secondRing[3].x = 0f;
		secondRing[3].y = -8f;

		// TODO Temp Create network second ring location, around nest
		RottenApple[] tempApples = new RottenApple[secondRing.Length];
		for(int i = 0; i < secondRing.Length; i++)
		{
			tempApples[i] = PrefabManager.instance.CreateAppleObject(secondRing[i]);
			tempApples[i].transform.SetParent(transform);
			
			// Create network connections
			CreateNetworkPath(tempLocs[i], tempApples[i]);
			CreateNetworkPath(tempLocs[i+1], tempApples[i]);
		}

		return this.nest;
	}

	/// <summary>
    /// The function creates a new straight path object that connects location A to location B.
	/// </summary>
    /// <param name="locationA">Location A of the new path.</param>
    /// <param name="locationB">Location B of the new path.</param>
    private void CreateNetworkPath(Location locationA, Location locationB)
	{
        StraightPath tempPath = PrefabManager.instance.CreateStraightPathObject();
		tempPath.transform.SetParent(transform);

        this.LocationNetwork.ConnectLocations(locationA, locationB, tempPath);
	}

	/// <summary>
	/// Gets the route to nest from the given location.
	/// </summary>
	/// <returns>The route to nest from the given location</returns>
	/// <param name="startLocation">The location to find a route starting from.</param>
	public Route GetRouteToNest(Location startLocation)
	{
		return this.LocationNetwork.GetRoute(startLocation, this.nest);
	}
}
