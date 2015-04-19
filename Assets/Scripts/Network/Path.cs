using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Path objects represent the connections bewteen locations.
/// </summary>
public abstract class Path : MonoBehaviour 
{
	protected Queue<Ant> waitingOnSideA;
	protected Queue<Ant> waitingOnSideB;

	protected Queue<Ant> travelingOnSideA;
	protected Queue<Ant> travelingOnSideB;

    /// <summary>
    /// The estimated cost of taking this path
    /// </summary>
    public int Cost;

    /// <summary>
    /// Location A of the path
    /// </summary>
    protected Location _locA;
    /// <summary>
    /// Gets Location A of the path.
    /// </summary>
    /// <value>A reference to Location A.</value>
    public Location LocationA
    {
        get
        {
            return _locA;
        }
    }

    /// <summary>
    /// Location B of the path
    /// </summary>
    protected Location _locB;
    /// <summary>
    /// Gets Location B of the path.
    /// </summary>
    /// <value>A reference to Location B.</value>
    public Location LocationB
    {
        get
        {
            return _locB;
        }
    }

	public virtual void Init()
	{
		waitingOnSideA = new Queue<Ant>();
		waitingOnSideB = new Queue<Ant>();
		travelingOnSideA = new Queue<Ant>();
		travelingOnSideB = new Queue<Ant>();

		enabled = false;
	}

	// Use this for initialization
	protected void Start () 
	{
	
	}
	
	// Update is called once per frame
    protected void Update () 
	{
		// Check for and move any traveling ants
		if(travelingOnSideA.Count != 0)
		{
			UpdateTravelAPosition();
		}

		if(travelingOnSideB.Count != 0)
		{
			UpdateTravelBPosition();
		}
	}

	protected abstract void UpdateTravelAPosition();

	protected abstract void UpdateTravelBPosition();

	protected abstract void StartTravelAPosition(Ant ant);

	protected abstract void StartTravelBPosition(Ant ant);

    /// <summary>
    /// This function connects the two locations, and configures the path for the connection.
    /// </summary>
    /// <param name="locationA">Location A of the path.</param>
    /// <param name="locationB">Location B of the path.</param>
    public void ConnectLocations(Location locationA, Location locationB)
    {
        if(_locA != null || _locB != null)
        {
            Debug.Log("Path.ConnectLocations(): ERROR - Path already connected");
            return;
        }

        _locA = locationA;
        _locB = locationB;

        locationA.AddConnection(locationB, this);
        locationB.AddConnection(locationA, this);

        CalculateDimensions();
    }

    /// <summary>
    /// Calculates the dimensions for a the path object bewteen locations A and B.
    /// </summary>
    protected abstract void CalculateDimensions();

	public virtual void TakePathFrom(Ant ant, Location locEnteringFrom)
	{
		if(locEnteringFrom == LocationA)
		{
			waitingOnSideA.Enqueue(ant);
		}
		else if(locEnteringFrom == LocationB)
		{
			waitingOnSideB.Enqueue(ant);
		}
		else
		{
			Debug.Log("Path: ERROR - Entering from unknown location");
		}

		CheckUpdateStatus();
	}

	protected virtual void FinishPathSideA(Ant ant)
	{
		// No longer needed if all queues are empty
		if((waitingOnSideA.Count == 0) &&
		   (waitingOnSideB.Count == 0) &&
		   (travelingOnSideA.Count == 0) &&
		   (travelingOnSideB.Count == 0))
		{
			StopAllCoroutines();
			enabled = false;
		}

		ant.Hide();
		LocationB.Enter(ant);
	}

	protected virtual void FinishPathSideB(Ant ant)
	{
		// No longer needed if all queues are empty
		if((waitingOnSideA.Count == 0) &&
		   (waitingOnSideB.Count == 0) &&
		   (travelingOnSideA.Count == 0) &&
		   (travelingOnSideB.Count == 0))
		{
			StopAllCoroutines();
			enabled = false;
		}
		
		ant.Hide();
		LocationA.Enter(ant);
	}

	protected void CheckUpdateStatus()
	{
		if(enabled)
		{
			return;
		}

		enabled = true;

		StartCoroutine(PathTravelCheck());
	}

	protected IEnumerator PathTravelCheck()
	{
		// TODO Can check the end things in the traveling queue if they are out of range
		// TODO Can trasnform into local coordinate to make movement easy
		// TODO Could put this in the General update function?
		Ant tempAnt;
		for(;;)
		{
			if(waitingOnSideA.Count != 0)
			{
				tempAnt = waitingOnSideA.Dequeue();
				travelingOnSideA.Enqueue(tempAnt);
				StartTravelAPosition(tempAnt);
				tempAnt.Unhide();
			}
			
			if(waitingOnSideB.Count != 0)
			{
				tempAnt = waitingOnSideB.Dequeue();
				travelingOnSideB.Enqueue(tempAnt);
				StartTravelBPosition(tempAnt);
				tempAnt.Unhide();
			}

			// TODO determine this time programatically
			yield return new WaitForSeconds(.2f);
		}
	}
}
