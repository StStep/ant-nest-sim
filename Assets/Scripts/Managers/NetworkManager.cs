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
	[HideInInspector]private Nest nest;

	//TODO this a temp creation method 
	public Vector2[] appleVecList;

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

		CreateNetwork();
	}

    void Start() 
    {
        GameManager.instance.nestAssignment.LocationConnect(nest);
    }

	/// <summary>
	/// This function initializes and forms the location network.
	/// </summary>
	private void CreateNetwork()
	{
		nest = PrefabManager.instance.CreateNestObject(Vector2.zero);
		nest.transform.SetParent(transform);
		LocationNetwork = new Network();
		
		// TODO Temp Create network location
		RottenApple[] tempApples = new RottenApple[appleVecList.Length];
		for(int i = 0; i < appleVecList.Length; i++)
		{
			tempApples[i] = PrefabManager.instance.CreateAppleObject(appleVecList[i]);
			tempApples[i].transform.SetParent(transform);

            // Create network connections
            CreateNetworkPath(nest, tempApples[i]);
        }


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

        LocationNetwork.ConnectLocations(locationA, locationB, tempPath);
	}
}
