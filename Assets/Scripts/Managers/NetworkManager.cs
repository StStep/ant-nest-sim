using UnityEngine;
using System.Collections;

/// <summary>
/// This class is a signleton used for creating and providing an interface to the
/// node network.
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
	/// specific nodes in the network.
	/// </summary>
	/// <returns>The next location I.</returns>
	public static int GetNextLocID() {
		return (nextLocID++);
	}

	/// <summary>
	/// The network of the nodes referencing the locations.
	/// </summary>
	[HideInInspector]public Network LocationNetwork;

	/// <summary>
	/// The Nest, the primary node of the network.
	/// </summary>
	public Nest nest;

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

	/// <summary>
	/// This function initializes and forms the location network.
	/// </summary>
	private void CreateNetwork()
	{
		nest = PrefabManager.instance.CreateNest(Vector2.zero);
		nest.transform.SetParent(transform);
		LocationNetwork = new Network(nest.networkNode);
		
		// TODO Temp Create network
		RottenApple[] tempApples = new RottenApple[appleVecList.Length];
		for(int i = 0; i < appleVecList.Length; i++)
		{
			tempApples[i] = PrefabManager.instance.CreateApple(appleVecList[i]);
			tempApples[i].transform.SetParent(transform);
		}

		CreateNetworkPath(nest, tempApples);
	}

	/// <summary>
	/// The function creates a path gameobject from the parent node to the children nodes.
	/// </summary>
	/// <param name="parentNode">The parent node to connect to each child node.</param>
	/// <param name="childrenNodes">The children node to connect to the parent.</param>
	private void CreateNetworkPath(Location parentNode, params Location[] childrenNodes)
	{
		GameObject temPath;
		foreach(Location location in childrenNodes)
		{
			LocationNetwork.ConnectNodes(parentNode.networkNode, location.networkNode);
			temPath = PrefabManager.instance.CreatePath(parentNode.transform.position, location.transform.position);
			temPath.transform.SetParent(transform);
		}
	}
}
