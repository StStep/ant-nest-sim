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
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void LetVisit(Ant ant)
	{
		// Return the ant from where it came
		Location temp = ant.destination;
		ant.destination = ant.origin;
		ant.origin = temp;

		// Give Ant food
		if(ant.foodCarrying < Ant.carryCapacity && foodAvailable > 0f)
		{
			float foodGiving = Ant.carryCapacity - ant.foodCarrying;
			foodGiving = (foodGiving < foodAvailable) ? foodGiving : foodAvailable;
			foodAvailable -= foodGiving;
			ant.foodCarrying += foodGiving;
		}

		Accept(ant);
	}

	protected override void UpdateText()
	{
		lowerText.text = ((int)foodAvailable) + " Food";
	}
}
