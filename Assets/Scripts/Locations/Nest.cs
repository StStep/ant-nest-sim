using UnityEngine;
using System.Collections;

/// <summary>
/// This calls represents the Nest game location
/// </summary>
public class Nest : Location {

	/// <summary>
	/// The amount of worker ants in the nest
	/// </summary>
	protected int workerAntCount;

	/// <summary>
	/// This initializes this instance, for intializaing internal objects 
	/// before Awake() is called.
	/// </summary>
	public override void Init()
	{
		base.Init();

		workerAntCount = 100;
		locationText.text = workerAntCount + " Workers";
	}

	protected override void Awake()
	{
		base.Awake();
	}

	/// <summary>
	/// This function should be called when a mouse clicks
	/// up after clicking on the game object.
	/// </summary>
	public override void ClickUp()
	{
		if (clickStatus == ClickType.LeftClick)
		{
			Debug.Log("You left clicked on a nest");
		}
		else if (clickStatus == ClickType.RightClick)
		{
			Debug.Log("You right clicked on a nest");
		}
	}

	/// <summary>
	/// This function removes workers up to the paramter, if able, 
	/// and returns the amount removed.
	/// </summary>
	/// <returns>The amount of workers removed fromt the Nest.</returns>
	/// <param name="amount">The amount of workers to try and remove from
	/// the Nest.</param>
	public int TakeWorkers(int amount)
	{

		if(amount < 0)
		{
			return 0;
		}

		int takeAmount = 0;
		if(workerAntCount >= amount)
		{
			workerAntCount -= amount;
			takeAmount = amount;
		}
		else
		{
			takeAmount = workerAntCount;
			workerAntCount = 0;
		}

		// Update worker count
		locationText.text = workerAntCount + " Workers";
		return takeAmount;
	}

	/// <summary>
	/// This function gives workers to the nest.
	/// </summary>
	/// <returns>The amount of workers given.</returns>
	/// <param name="amount">The amount of workers to try anf give to the nest</param>
	public int GiveWorkers(int amount)
	{
		if(amount < 0)
		{
			return 0;
		}

		// Update worker count
		workerAntCount += amount;
		locationText.text = workerAntCount + " Workers";
		return amount;
	}
}
