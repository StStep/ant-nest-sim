using UnityEngine;
using System.Collections;

/// <summary>
/// This is the ant that collects food for the nest.
/// </summary>
public class WorkerAnt : Ant , IStreamAnt 
{

	/// <summary>
	/// Initialize the ant gameobject, this should be called
	/// after instantiation.
	/// </summary>
	public override void Init()
	{
		base.Init();
	}

	/// <summary>
	/// This function restarts the ant processes. This should
	/// be called when reusing the ant from a ant object pool.
	/// </summary>
	public override void Birth()
	{
		base.Birth();
	}

	// Use this for initialization
	protected override void Awake() 
	{
		base.Awake();
	
	}
	
	// Update is called once per frame
	protected override void  Update() 
	{
		base.Update();
	}
}
