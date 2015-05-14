using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Path objects represent the connections bewteen locations.
/// </summary>
public abstract class Path : MonoBehaviour
{
	protected const float ExpRollingFilterAlpha = 0.2f;

    /// <summary>
    /// The estimated cost of taking this path
    /// </summary>
    public int Cost;

	public Text waitingAText;
	public Text waitingBText;
	public Text travelingAText;
	public Text travelingBText;
	public Text rateText;
	public SpriteRenderer animationTravelA;
	public SpriteRenderer animationTravelB;

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

	protected Queue<Ant> waitingOnSideA;
	protected Queue<Ant> waitingOnSideB;

	protected List<Ant> travelingOnSideA;
	protected List<Ant> travelingOnSideB;

	/// <summary>
	/// The capacity if the path in Ants Per Second
	/// </summary>
	protected float _capacity;

	public float Capacity
	{
		get
		{
			return _capacity;
		}

		protected set
		{
			_capacity = value;
			_capPerTick = _capacity * GameManager.secondsPerGameTick;
		}
	}

	/// <summary>
	/// The capcity of the path per tick
	/// </summary>
	protected float _capPerTick;
	protected float _leftOverCapPerTick;
	protected int CapPerTick
	{
		// This allows for partial ants per tick to build up over ticks to eventually become a whole number
		get
		{
			int retInt = Mathf.FloorToInt(_capPerTick + _leftOverCapPerTick);
			_leftOverCapPerTick = (_capPerTick + _leftOverCapPerTick) - retInt;
			return retInt;
		}
	}
	
	/// <summary>
	/// The length of the path in m
	/// </summary>
	protected float pathLength;
	
	/// <summary>
	/// The number of seconds it takes an ant to traverse this path
	/// </summary>
	protected float secForAnt;

	protected float capTickAverage;

	public virtual void Init()
	{
		waitingOnSideA = new Queue<Ant>();
		waitingOnSideB = new Queue<Ant>();

		travelingOnSideA = new List<Ant>();
		travelingOnSideB = new List<Ant>();

		capTickAverage = 0;
	}

	// Use this for initialization
	protected void Start () 
	{
		enabled = false;
		UpdateText();

		animationTravelA.enabled = false;
		animationTravelB.enabled = false;
	}
	
	// Update is called once per frame
    protected void Update () 
	{
	}

	/// <summary>
	/// Take an amount from each side to move on the path that's
	/// a ratio of the ant counts on either side, up to the capacity.
	/// </summary>
	public void PathUpdate()
	{
		int fromSideA = 0;
		int fromSideB = 0;
		float ratio = 0f;
		int capThisTick = CapPerTick;

		// Take ratio of ants from both directions
		if(waitingOnSideA.Count == 0 && waitingOnSideB.Count == 0 &&
		   travelingOnSideA.Count == 0 && travelingOnSideB.Count == 0)
		{
			// Do nothing if there are no ants here, update only if neede
			if(capTickAverage != 0f)
			{
				capTickAverage = 0f;
				UpdateText();
			}

			return;
		}
		if(waitingOnSideA.Count == 0 && waitingOnSideB.Count == 0)
		{
			// StopAllCoroutines();
		}
		else if(waitingOnSideA.Count == 0 || waitingOnSideB.Count == 0)
		{
			fromSideA = (waitingOnSideA.Count == 0) ? 0 : capThisTick;
			fromSideB = (fromSideA == 0) ? capThisTick : 0;
		}
		else if(waitingOnSideB.Count < waitingOnSideA.Count)
		{
			ratio = ((float)waitingOnSideB.Count)/((float)waitingOnSideA.Count);
			fromSideB = Mathf.FloorToInt(ratio * capThisTick);
			fromSideA = capThisTick - fromSideB;
		}
		else
		{
			ratio = ((float)waitingOnSideA.Count)/((float)waitingOnSideB.Count);
			fromSideA = Mathf.FloorToInt(ratio * capThisTick);
			fromSideB = capThisTick - fromSideA;
		}

		// Move along traveling Queue of ants
		for(int i = travelingOnSideA.Count - 1; i >= 0; i--)
		{
			travelingOnSideA[i].travelTimeLeft -= GameManager.secondsPerGameTick;
			if(travelingOnSideA[i].travelTimeLeft < float.Epsilon)
			{
				LocationB.Accept(travelingOnSideA[i]);
				travelingOnSideA.RemoveAt(i);
			}
		}

		for(int i = travelingOnSideB.Count - 1; i >= 0; i--)
		{
			travelingOnSideB[i].travelTimeLeft -= GameManager.secondsPerGameTick;
			if(travelingOnSideB[i].travelTimeLeft < float.Epsilon)
			{
				LocationA.Accept(travelingOnSideB[i]);
				travelingOnSideB.RemoveAt(i);
			}
		}

		if(travelingOnSideA.Count > 0) 
		{
			animationTravelA.enabled = true;
		}
		else
		{
			animationTravelA.enabled = false;
		}

		if(travelingOnSideB.Count > 0) 
		{
			animationTravelB.enabled = true;
		}
		else
		{
			animationTravelB.enabled = false;
		}

		if(fromSideA > waitingOnSideA.Count)
		{
			fromSideA = waitingOnSideA.Count;
		}

		if(fromSideB > waitingOnSideB.Count)
		{
			fromSideB = waitingOnSideB.Count;
		}

		// Move ants to traveling queues
		Ant tempAnt;
		for(int i = 0; i < fromSideA; i++)
		{
			tempAnt = waitingOnSideA.Dequeue();
			tempAnt.travelTimeLeft = secForAnt;
			travelingOnSideA.Add(tempAnt);
		}

		for(int i = 0; i < fromSideB; i++)
		{
			tempAnt = waitingOnSideB.Dequeue();
			tempAnt.travelTimeLeft = secForAnt;
			travelingOnSideB.Add(tempAnt);
		}

		// Calculate Exponential Moving Average of capacity per tick
		capTickAverage = (ExpRollingFilterAlpha * (fromSideA + fromSideB)) + (1.0f - ExpRollingFilterAlpha) * capTickAverage;

		UpdateText();
	}
	
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

	public void AcceptFrom(Location from, Ant ant)
	{
		if(from == LocationA)
		{
			waitingOnSideA.Enqueue(ant);
		}
		else if(from == LocationB)
		{
			waitingOnSideB.Enqueue(ant);
		}
		else
		{
			Debug.Log("AcceptFrom: Path bewteen LocID " + LocationA.LocID + " and LocID " + 
			          LocationB.LocID + "acception from invalid location ID " + from.LocID);
		}
	}

	protected void UpdateText()
	{
		travelingAText.text = "Travel (" + travelingOnSideA.Count + ")";
		travelingBText.text = "Travel (" + travelingOnSideB.Count + ")";
		waitingAText.text = "Wait (" + waitingOnSideA.Count + ") --}";
		waitingBText.text = "{-- Wait (" + waitingOnSideB.Count + ")";
		rateText.text = "Rate " + (capTickAverage/GameManager.secondsPerGameTick).ToString("0.0") + "/" + Capacity.ToString("0.0");
	}
}
