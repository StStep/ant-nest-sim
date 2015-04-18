using UnityEngine;
using System.Collections;

/// <summary>
/// A Straight Path is a straight line btween two locations.
/// </summary>
public class StraightPath : Path {

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
    }
}
