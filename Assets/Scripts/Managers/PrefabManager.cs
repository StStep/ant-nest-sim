using UnityEngine;
using System.Collections;

/// <summary>
/// This class is a signleton used for holding references to and creating
/// game objects from prefabs.
/// </summary>
public class PrefabManager : MonoBehaviour {

	/// <summary>
	/// The single current instance of this class
	/// </summary>
	public static PrefabManager instance;

	// TODO Generalize and organize
	public Nest nestPrefab;
	public RottenApple applePrefab;
	public GameObject pathPrefab;
	public WorkerAnt workerAntPrefab;

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

	/// <summary>
	/// Creates a Nest prefab at the given position,
	/// </summary>
	/// <returns>The spawned Nest gameobject</returns>
	/// <param name="position">The position to spawn the gameobject at.</param>
	public Nest CreateNest(Vector2 position)
	{
		Nest newNest = Instantiate(nestPrefab, position, Quaternion.identity) as Nest;
		newNest.Init();
		return newNest;
	}

	/// <summary>
	/// Creates a RottenApple prefab at the given position,
	/// </summary>
	/// <returns>The spawned RottenApple gameobject</returns>
	/// <param name="position">The position to spawn the gameobject at.</param>
	public RottenApple CreateApple(Vector2 position)
	{
		RottenApple newApple = Instantiate(applePrefab, position, Quaternion.identity) as RottenApple;
		newApple.Init();
		return newApple;
	}
	
	// TODO Path can use nodeLocation info of nodes?

	/// <summary>
	/// Creates a Path prefab connected the nodes at the given positions,
	/// </summary>
	/// <returns>The spawned Path gameobject</returns>
	/// <param name="startPosition">The position of the node to start the path at.</param>
	/// <param name="endPosition">THe position of the node that the path goes to.</param>
	/// <para>
	/// This function spawns a Path gameobject with the proper position, rotation,  
	/// and scale to connect the nodes at the provided positions
	/// </para>
	public GameObject CreatePath(Vector2 startPosition, Vector2 endPosition)
	{
		// Calculate middle point and rotation bewteen positions
		Vector2 direction = endPosition - startPosition;
		Vector2 position = (direction/2f) + startPosition;
		float rotAngle = Vector2.Angle(direction, Vector2.right);
		Quaternion rotation = Quaternion.Euler(0, 0, -rotAngle);

		// Create object, no roation until after scaling
		GameObject newPath = Instantiate(pathPrefab, position, Quaternion.identity) as GameObject;

		// Calculate the new scale, for the distance between the two node circles
		float currentSize = newPath.GetComponent<SpriteRenderer>().bounds.size.x;
		Vector3 scale = newPath.transform.localScale;
		float targetSize = direction.magnitude - .90f; // The node circle radii are .45f
		scale.x = targetSize * scale.x / currentSize;
		newPath.transform.localScale = scale;

		// Rotate after scaling
		newPath.transform.rotation = rotation;

		return newPath;
	}

	/// <summary>
	/// Creates a WorkerAnt prefab at the given position,
	/// </summary>
	/// <returns>The spawned WorkerAnt gameobject</returns>
	/// <param name="position">The position to spawn the gameobject at.</param>
	public WorkerAnt CreateWorkerAnt(Vector2 position)
	{
		WorkerAnt newWorker = Instantiate(workerAntPrefab, position, Quaternion.identity) as WorkerAnt;
		newWorker.Init();
		return newWorker;
	}

}
