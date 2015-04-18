using UnityEngine;
using System.Collections;

/// <summary>
/// Path objects represent the connections bewteen locations.
/// </summary>
public abstract class Path : MonoBehaviour 
{
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

	// Use this for initialization
	protected void Start () {
	
	}
	
	// Update is called once per frame
    protected void Update () {
	
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
}
