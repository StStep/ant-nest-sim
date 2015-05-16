using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A Straight Path is a straight line btween two locations.
/// </summary>
public class StraightPath : Path {

	public const float StraightPathCapacity = 8;

	public override void Init ()
	{
		base.Init ();
		Capacity = StraightPathCapacity;
	}

    /// <summary>
    /// Calculates the dimensions for a straight line between locations A and B.
    /// </summary>
    protected override void CalculateDimensions()
    {

        // Calculate middle point and rotation bewteen positions
        Vector2 direction = LocationB.Position - LocationA.Position;
		pathLength = direction.magnitude;
		secForAnt = pathLength*Ant.timeToMoveOneUnit;
        Vector2 position = (direction/2f) + LocationA.Position;

		float rotAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(rotAngle, Vector3.forward);

        transform.position = position;
        
        // Calculate the new scale, for the distance between the two node circles
		SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
		float currentSize = renderer.bounds.size.x;
		Vector3 scale = renderer.transform.localScale;
        float targetSize = direction.magnitude - .90f*1.5f; // The node circle radii are .45f with 1.5f scale
        scale.x = targetSize * scale.x / currentSize;
		renderer.transform.localScale = scale;
        
        // Rotate after scaling
		transform.rotation = rotation;
    }
}
