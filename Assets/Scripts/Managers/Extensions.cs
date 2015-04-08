using UnityEngine;
using System.Collections;

/// <summary>
/// A static calls of extensions.
/// </summary>
public static class Extensions
{
	public static void SetPositionX(this Transform t, float newX)
	{
		t.position = new Vector3(newX, t.position.y, t.position.z);
	}
	
	public static void SetPositionY(this Transform t, float newY)
	{
		t.position = new Vector3(t.position.x, newY, t.position.z);
	}
	
	public static void SetPositionZ(this Transform t, float newZ)
	{
		t.position = new Vector3(t.position.x, t.position.y, newZ);
	}
	
	public static float GetPositionX(this Transform t)
	{
		return t.position.x;
	}
	
	public static float GetPositionY(this Transform t)
	{
		return t.position.y;
	}
	
	public static float GetPositionZ(this Transform t)
	{
		return t.position.z;
	}

	public static float GetAngleX(this Transform t)
	{
		return t.eulerAngles.x;
	}
	
	public static float GetAngleY(this Transform t)
	{
		return t.eulerAngles.y;
	}
	
	public static float GetAngleZ(this Transform t)
	{
		return t.eulerAngles.z;
	}

	public static void SetAngleX(this Transform t, float newX)
	{
		t.eulerAngles = new Vector3(newX, t.eulerAngles.y, t.eulerAngles.z);
	}
	
	public static void SetAngleY(this Transform t, float newY)
	{
		t.eulerAngles = new Vector3(t.eulerAngles.x, newY, t.eulerAngles.z);
	}
	
	public static void SetAngleZ(this Transform t, float newZ)
	{
		t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, newZ);
	}

	public static void SetPositionWithVector2(this Transform t, Vector2 newPos, float newZ)
	{
		t.position = new Vector3(newPos.x, newPos.y, newZ);
	}
	
	public static bool HasRigidbody(this GameObject gobj)
	{
		return (gobj.GetComponent<Rigidbody>() != null);
	}
	
	public static bool HasAnimation(this GameObject gobj)
	{
		return (gobj.GetComponent<Animation>() != null);
	}
	
	public static void SetSpeed(this Animation anim, float newSpeed)
	{
		anim[anim.clip.name].speed = newSpeed; 
	}

	/// <summary>
	/// Create a new Vector3 from the contents extended Vector2 with a given float for z.
	/// </summary>
	/// <returns>The created Vector3.</returns>
	/// <param name="vec2">The Vector to copy x and y from.</param>
	/// <param name="newZ">The new z to use.</param>
	public static Vector3 ToVec3(this Vector2 vec2, float newZ)
	{
		return new Vector3(vec2.x, vec2.y, newZ);
	}
}