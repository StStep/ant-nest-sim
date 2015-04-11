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

	/// <summary>
	/// This couroutine function allows an ant to enter a location. This couroutine should be called 
	/// by the ant when the ant wants to enter a node. If the ant is a worker it takes food when entering.
	/// </summary>
	/// <param name="enteringAnt">The ant that is entering the location</param>
	public override IEnumerator Enter(Ant enteringAnt)
	{
		enteringAnt.Hide();

		// Currently just a random wait
		float waitTime = Random.Range(0.1f, 1f);
		yield return new WaitForSeconds(waitTime);

		// If worker, take what food the ant can 
		if(enteringAnt is WorkerAnt)
		{
			foodAvailable -= ((WorkerAnt) enteringAnt).Take(CarryType.Food, foodAvailable);
			lowerText.text = ((int)foodAvailable) + " Food";
		}
	}
	
	/// <summary>
	/// This couroutine function allows an ant to exit a location. This couroutine should be called 
	/// by the ant when the ant wants to leave a node.
	/// </summary>
	/// <param name="exitingAnt">The ant that is exiting the location</param>
	public override IEnumerator Exit(Ant exitingAnt)
	{
		exitingAnt.Unhide();

		// Currently just a random wait
		float waitTime = Random.Range(0.1f, 1f);
		yield return new WaitForSeconds(waitTime);
	}
}
