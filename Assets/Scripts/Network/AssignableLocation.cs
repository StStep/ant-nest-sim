using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class is for locations where ants are assigned to.
/// </summary>
public abstract class AssignableLocation : Location 
{
    public bool haveAssignment;

	/// <summary>
	/// This initializes this instance, for intializaing internal objects 
	/// before Awake() is called.
	/// </summary>
	public override void Init()
	{
		base.Init();
		
        haveAssignment = false;
	}
	
	protected override void Awake()
	{
		base.Awake();
	}
	
	/// <summary>
	/// This function should be called when a mouse clicks
	/// up after clicking on the game object.
	/// </summary>
	/// <para>
	/// TODO
	/// </para>
	public override void ClickUp()
	{
		
		if (clickStatus == ClickType.LeftClick)
		{
            Debug.Log ("Left clicked on Rotten Apple ID " + LocID.ToString());
            GameManager.instance.LeftClick(this);
		}
		else if (clickStatus == ClickType.RightClick)
		{
            Debug.Log ("Right clicked on Rotten Apple ID " + LocID.ToString());
            GameManager.instance.RightClick(this);
		}
	}
}
