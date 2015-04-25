﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO make manager an inherited class

/// <summary>
/// This class is a singleton used for game management.
/// </summary>
public class GameManager : MonoBehaviour {

    public const int startAntAmount = 100;
	public const int antAssignIncrements = 1;
	public const float secondsPerGameTick = 0.5f;

	/// <summary>
	/// The single current instance of this class
	/// </summary>
	public static GameManager instance;

    private Dictionary<int, StreamAssignment> streamAssignmentDict;

    private Queue<StreamAssignment> streamPool;

	private Nest nest;

	protected void Awake()
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

        streamAssignmentDict = new Dictionary<int, StreamAssignment>();
        streamPool = new Queue<StreamAssignment>();
	}

	protected void Start()
	{
		nest = NetworkManager.instance.CreateNetwork();
		nest.antsInNest = startAntAmount;
		nest.upperText.text = nest.antsInNest + " ants";
		StartCoroutine(GameUpdate());
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

		stream.AssignAnts(antAssignIncrements);
    }
    
    public void RightClick(AssignableLocation location)
    {
        if(!location.haveAssignment)
        {
			return;
        }

		StreamAssignment stream = streamAssignmentDict[location.LocID];

		stream.UnassignAnts(antAssignIncrements);
		if(stream.AntsPerSec == 0)
		{
			DisconnectStreamAssignment(location);
			location.haveAssignment = false;
		}
    }

	protected IEnumerator GameUpdate()
	{
		for(;;)
		{
			// Send ant packets for each stream assignment
			foreach(StreamAssignment entry in streamAssignmentDict.Values)
			{
				nest.SendAntsTo(entry.AntsPerTick, entry.AssignedLocation);
			}

			// Update nest count
			nest.upperText.text = nest.antsInNest + " ants";

			// Update Network
			NetworkManager.instance.LocationNetwork.NetworkUpdate();

			yield return new WaitForSeconds(secondsPerGameTick);
		}
	}


}
