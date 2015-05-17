using UnityEngine;
using System.Collections;

public class InterfaceManager : MonoBehaviour {

	private AssignableLocation selectedLoc;
	private Assignment selectedAssign;

	public Canvas locationMenu;

	public Canvas assignmentMenu;

	/// <summary>
	/// The single current instance of this class
	/// </summary>
	public static InterfaceManager instance;
	protected void Awake()
	{
		// Only have one in game
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this) 
		{
			Destroy(gameObject);	
		}
		
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		selectedLoc = null;
		selectedAssign = null;
		locationMenu.enabled = false;
		assignmentMenu.enabled = false;
	}
	
	protected void Start()
	{
	}

	public void SelectLocation(AssignableLocation loc)
	{
		if(loc != selectedLoc)
		{
			Deselect();

			selectedLoc = loc;
			locationMenu.enabled = true;
			locationMenu.transform.position = selectedLoc.transform.position;
		}
	}

	public void SelectAssignment(AssignableLocation loc, Assignment assign)
	{
		if(loc != selectedLoc)
		{
			Deselect();

			selectedLoc = loc;
			selectedAssign = assign;
			assignmentMenu.enabled = true;
			assignmentMenu.transform.position = selectedLoc.transform.position;
		}
	}

	public void Deselect()
	{
		if(selectedLoc != null)
		{
			locationMenu.enabled = false;
			selectedLoc = null;
		}

		if(selectedAssign != null)
		{
			selectedAssign.Deselect();
			assignmentMenu.enabled = false;
			selectedAssign = null;
		}
	}


	public void CreateStreamAssignmentForSelectedLoc()
	{
		if(selectedLoc != null)
		{
			selectedAssign = selectedLoc.CreateStreamAssignment();
			assignmentMenu.enabled = true;
			assignmentMenu.transform.position = selectedLoc.transform.position;
			locationMenu.enabled = false;
		}
	}

	public void RemoveAssignForSelectedLoc()
	{
		if(selectedAssign != null && selectedLoc != null)
		{
			selectedLoc.RemoveStreamAssignment();
			selectedAssign = null;
			assignmentMenu.enabled = false;
			locationMenu.transform.position = selectedLoc.transform.position;
			locationMenu.enabled = true;
		}
	}

	public void IncrementAssign()
	{
		if(selectedAssign != null)
		{
			selectedAssign.AssignAnts(1);
		}
	}

	public void DecrementAssign()
	{
		if(selectedAssign != null)
		{
			selectedAssign.UnassignAnts(1);
		}
	}
}
