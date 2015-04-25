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

	//  this a temp creation method 
	public Vector2[] appleVecList;

	// TODO this is a temp creation method for testinh
	public Vector2[] childrenVecList;

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
		
		// TODO Temp Create network location, around nest
		RottenApple[] tempApples = new RottenApple[appleVecList.Length];
		for(int i = 0; i < appleVecList.Length; i++)
		{
			tempApples[i] = PrefabManager.instance.CreateAppleObject(appleVecList[i]);
			tempApples[i].transform.SetParent(transform);

            // Create network connections
			CreateNetworkPath(this.nest, tempApples[i]);
        }

		// TODO Temp Create children nodes on first nest connected apple
		RottenApple[] tempChildApples = new RottenApple[childrenVecList.Length];
		for(int i = 0; i < childrenVecList.Length; i++)
		{
			tempChildApples[i] = PrefabManager.instance.CreateAppleObject(childrenVecList[i]);
			tempChildApples[i].transform.SetParent(transform);
			
			// Create network connections
			CreateNetworkPath(tempApples[0], tempChildApples[i]);
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
