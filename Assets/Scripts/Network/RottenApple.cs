using UnityEngine;
using System.Collections;

/// <summary>
/// This class represents food location with a rotten apple available.
/// </summary>
public class RottenApple : AssignableLocation {

	protected float foodAvailable;

	/// <summary>
	/// This initializes this instance, for intializaing internal objects 
	/// before Awake() is called.
	/// </summary>
	public override void Init()
	{
		base.Init();

		foodAvailable = 1000f;

		lowerText.text = ((int)foodAvailable) + " Food";
	}

	protected override void Awake()
	{
		base.Awake();
	}
}
