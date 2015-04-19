using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A Straight Path is a straight line btween two locations.
/// </summary>
public class StraightPath : Path {

	private const float TravelFromPathOffset = .1f;
	protected Vector2 travelFromADirection;
	protected Vector2 travelFromBDirection;
	protected Vector2 startOnAPosition;
	protected Vector2 startOnBPosition;
	protected Vector2 endOnAPosition;
	protected Vector2 endOnBPosition;
	protected float angleASide;
	protected float angleBSide;

    /// <summary>
    /// Calculates the dimensions for a straight line between locations A and B.
    /// </summary>
    protected override void CalculateDimensions()
    {

        // Calculate middle point and rotation bewteen positions
        Vector2 direction = LocationB.Position - LocationA.Position;
        Vector2 position = (direction/2f) + LocationA.Position;
        float rotAngle = Vector2.Angle(direction, Vector2.right);
        Quaternion rotation = Quaternion.Euler(0, 0, -rotAngle);

        transform.position = position;
        
        // Calculate the new scale, for the distance between the two node circles
        float currentSize = GetComponent<SpriteRenderer>().bounds.size.x;
        Vector3 scale = transform.localScale;
        float targetSize = direction.magnitude - .90f; // The node circle radii are .45f
        scale.x = targetSize * scale.x / currentSize;
        transform.localScale = scale;
        
        // Rotate after scaling
        transform.rotation = rotation;

		// Calculate Traveling vectors
		travelFromADirection = LocationB.Position - LocationA.Position;
		travelFromADirection.Normalize();

		travelFromBDirection = LocationA.Position - LocationB.Position;
		travelFromBDirection.Normalize();

		// Create perpendicular and then offset from location in both direction
		// from location and perpepdicular to that
		Vector2 startOnAOffset = new Vector2(travelFromADirection.y, -travelFromADirection.x);
		startOnAPosition = (startOnAOffset + travelFromADirection ) * TravelFromPathOffset + LocationA.Position;
		Vector2 endOnAOffset = new Vector2(-travelFromADirection.y, travelFromADirection.x);
		endOnAPosition = (endOnAOffset + travelFromADirection ) * TravelFromPathOffset + LocationA.Position;

		angleASide = Mathf.Atan2(travelFromADirection.y, travelFromADirection.x) * Mathf.Rad2Deg;
		if(angleASide < 0) angleASide += 360;

		Vector2 startOnBOffset = new Vector2(travelFromBDirection.y, -travelFromBDirection.x);
		startOnBPosition = (startOnBOffset + travelFromBDirection) * TravelFromPathOffset + LocationB.Position;
		Vector2 endOnBOffset = new Vector2(-travelFromBDirection.y, travelFromBDirection.x);
		endOnBPosition = (endOnBOffset + travelFromBDirection ) * TravelFromPathOffset + LocationB.Position;

		angleBSide = Mathf.Atan2(travelFromBDirection.y, travelFromBDirection.x) * Mathf.Rad2Deg;
		if(angleBSide < 0) angleBSide += 360;
    }

	protected override void UpdateTravelAPosition()
	{
		foreach(Ant ant in travelingOnSideA)
		{
			//Find a new position proportionally closer to the end, based on the moveTime
			Vector3 newPostion = Vector3.MoveTowards(ant.transform.position, endOnBPosition, Ant.inverseMoveTime * Time.deltaTime);
			ant.transform.position = newPostion;
		}

		// Check the position of the farthest along ant
		Ant firstAnt = travelingOnSideA.Peek();
		float sqrRemainingDistance = ((Vector2)firstAnt.transform.position - endOnBPosition).sqrMagnitude;
		
		if(sqrRemainingDistance <= float.Epsilon)
		{
			travelingOnSideA.Dequeue();
			FinishPathSideA(firstAnt);
		}
	}
	
	protected override void UpdateTravelBPosition()
	{
		foreach(Ant ant in travelingOnSideB)
		{
			//Find a new position proportionally closer to the end, based on the moveTime
			Vector3 newPostion = Vector3.MoveTowards(ant.transform.position, endOnAPosition, Ant.inverseMoveTime * Time.deltaTime);
			ant.transform.position = newPostion;
		}
		
		// Check the position of the farthest along ant
		Ant firstAnt = travelingOnSideB.Peek();
		float sqrRemainingDistance = ((Vector2)firstAnt.transform.position - endOnAPosition).sqrMagnitude;
		
		if(sqrRemainingDistance <= float.Epsilon)
		{
			travelingOnSideB.Dequeue();
			FinishPathSideB(firstAnt);
		}
	}

	protected override void StartTravelAPosition(Ant ant)
	{
		ant.transform.position = startOnAPosition;
		ant.transform.SetAngleZ(angleASide);
	}
	
	protected override void StartTravelBPosition(Ant ant)
	{
		ant.transform.position = startOnBPosition;
		ant.transform.SetAngleZ(angleBSide);
	}
}
