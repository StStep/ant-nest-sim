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
    public StraightPath straightPathPrefab;
	public WorkerAnt workerAntPrefab;
    public NestAssignment nestAssignmentPrefab;
    public StreamAssignment streamAssignmentPrefab;

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
	public Nest CreateNestObject(Vector2 position)
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
	public RottenApple CreateAppleObject(Vector2 position)
	{
		RottenApple newApple = Instantiate(applePrefab, position, Quaternion.identity) as RottenApple;
		newApple.Init();
		return newApple;
	}

	/// <summary>
	/// Creates a SraightPath prefab without any connections.
	/// </summary>
    /// <returns>The spawned SraightPath gameobject</returns>
	/// <para>
    /// This function spawns a SraightPath gameobject without any connections.
	/// </para>
    public StraightPath CreateStraightPathObject()
	{
        StraightPath newStraightPath = Instantiate(straightPathPrefab, Vector3.zero, Quaternion.identity) as StraightPath;
		newStraightPath.Init();
		return newStraightPath;
	}

	/// <summary>
	/// Creates a WorkerAnt prefab at the given position,
	/// </summary>
	/// <returns>The spawned WorkerAnt gameobject</returns>
	/// <param name="position">The position to spawn the gameobject at.</param>
	public WorkerAnt CreateWorkerAntObject(Vector2 position)
	{
		WorkerAnt newWorker = Instantiate(workerAntPrefab, position, Quaternion.identity) as WorkerAnt;
		newWorker.Init();
		return newWorker;
	}

    public NestAssignment CreateNestAssignment()
    {
        NestAssignment nestAssignment = Instantiate(nestAssignmentPrefab,  Vector3.zero, Quaternion.identity) as NestAssignment;
        nestAssignment.Init();
        return nestAssignment;
    }

    public StreamAssignment CreateStreamAssignment()
    {
        StreamAssignment streamAssignment = Instantiate(streamAssignmentPrefab,  Vector3.zero, Quaternion.identity) as StreamAssignment;
        streamAssignment.Init();
        return streamAssignment;
    }

}
