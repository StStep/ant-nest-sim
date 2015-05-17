using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class is for locations where ants are assigned to.
/// </summary>
public abstract class AssignableLocation : Location 
{
	protected Assignment assignment;

	/// <summary>
	/// This initializes this instance, for intializaing internal objects 
	/// before Awake() is called.
	/// </summary>
	public override void Init()
	{
		base.Init();
		
		assignment = null;
	}
	
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void LeftClick()
	{
		if(assignment == null)
		{
			InterfaceManager.instance.SelectLocation(this);
		}
		else
		{
			InterfaceManager.instance.SelectAssignment(this, assignment);
		}
	}

	protected override void RightClick()
	{
		InterfaceManager.instance.Deselect();
	}

	public Assignment CreateStreamAssignment()
	{
		if(assignment != null)
		{
			Debug.Log ("Warning: Assignment already exists");
		}
		else 
		{
			assignment = GameManager.instance.ConnectStreamAssignment(this);
		}

		return assignment;
	}

	public void RemoveStreamAssignment()
	{
		GameManager.instance.DisconnectStreamAssignment(this);
		assignment = null;
		upperText.text = "";
	}

	public virtual void UpdateAssignmentText(int amount)
	{
		upperText.text = amount + " ants";
	}
}
