using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public Transform cameraTrans;
	public float dragScale = .25f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// The potential click types on the gameobject.
	/// </summary>
	protected enum ClickType { NoClick, LeftClick, RightClick }
	
	/// <summary>
	/// The most recent click type on the gameobject
	/// </summary>
	protected ClickType clickStatus;

	protected Vector2 prevMousePos;
	
	public void DragStart()
	{
		prevMousePos = Input.mousePosition;
	}

	public void DragMove()
	{

		Vector2 curMosuePos = Input.mousePosition;
		Vector2 mouseMove = curMosuePos - prevMousePos;

		float newX = cameraTrans.position.x - mouseMove.x * dragScale;
		float newY = cameraTrans.position.y - mouseMove.y * dragScale;
		cameraTrans.position = new Vector3(newX, newY, cameraTrans.position.z);

		prevMousePos = Input.mousePosition;
	}

	public void DragEnd()
	{
	}
}
