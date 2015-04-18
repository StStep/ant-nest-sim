using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO make manager an inherited class

/// <summary>
/// This class is a singleton used for game management.
/// </summary>
public class GameManager : MonoBehaviour {

    public const int startWorkAntPool = 100;
	public const int workerAssignIncrements = 10;

	/// <summary>
	/// The single current instance of this class
	/// </summary>
	public static GameManager instance;

    public NestAssignment nestAssignment;

    private Dictionary<int, StreamAssignment> streamAssignmentDict;

    private Queue<StreamAssignment> streamPool;


    /// <summary>
    /// This is a pool of worker ant objects to pull from when birthing ants
    /// </summary>
    public Queue<WorkerAnt> workerAntPool;

	/// <summary>
	/// The next ant ID that will be assigned.
	/// </summary>
	private static int nextAntID = 0;
	
	/// <summary>
	/// This function request a unique ant ID for tracking
	/// specific ants in the game.
	/// </summary>
	/// <returns>The next ant ID.</returns>
	public static int GetNextAntID() {
		return (nextAntID++);
	}
	
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

        workerAntPool = new Queue<WorkerAnt>();
        streamAssignmentDict = new Dictionary<int, StreamAssignment>();
        streamPool = new Queue<StreamAssignment>();

        // Add starting ants to worker pool
        WorkerAnt tempAnt;
        for(int i = 0; i < startWorkAntPool; i++)
        {
            tempAnt = PrefabManager.instance.CreateWorkerAntObject(transform.position);
            workerAntPool.Enqueue(tempAnt);
            tempAnt.transform.parent = transform;
        }

        nestAssignment = PrefabManager.instance.CreateNestAssignment();
	}

    private StreamAssignment ConnectStreamAssignment(AssignableLocation location)
    {
        StreamAssignment newStream;
        if(streamPool.Count != 0)
        {
            newStream = streamPool.Dequeue();
        }
        else
        {
            newStream = PrefabManager.instance.CreateStreamAssignment();
        }
        newStream.LocationConnect(location);
        streamAssignmentDict.Add(location.LocID, newStream);
        return newStream;
    }

    private void DisconnectStreamAssignment(AssignableLocation location)
    {
        StreamAssignment stream = streamAssignmentDict[location.LocID];
        streamAssignmentDict.Remove(location.LocID);
        stream.LocationDisconnect();
        streamPool.Enqueue(stream);
        if(streamPool.Count > 10)
        {
            Debug.Log("(DEBUG) Stream pool is grater than 10");
        }
    }

    public void LeftClick(AssignableLocation location)
    {
		if(nestAssignment.Count == 0)
		{
			return;
		}

        StreamAssignment stream;
        if(!location.haveAssignment)
        {
            stream = ConnectStreamAssignment(location);
            location.haveAssignment = true;
        }
        else
        {
            stream = streamAssignmentDict[location.LocID];
        }

		stream.AssignWorkers(nestAssignment.UnassignWorkers(workerAssignIncrements));
    }
    
    public void RightClick(AssignableLocation location)
    {
        if(!location.haveAssignment)
        {
			return;
        }

		StreamAssignment stream = streamAssignmentDict[location.LocID];

		nestAssignment.AssignWorkers(stream.UnassignWorkers(workerAssignIncrements));
		if(stream.Count == 0)
		{
			DisconnectStreamAssignment(location);
			location.haveAssignment = false;
		}
    }
    
    public void HandleAntDeath(WorkerAnt deadAnt)
    {
/*        deadAnt.assignedLocation.RemoveAnt(deadAnt);
        if(deadAnt is WorkerAnt)
        {
            workerAntPool.Enqueue((WorkerAnt)deadAnt);
        }
        else
        {
            Debug.Log("HandleAntDeath: ERROR - Unknown ant class");
        }
        deadAnt.transform.position = transform.position;*/
    }
}
