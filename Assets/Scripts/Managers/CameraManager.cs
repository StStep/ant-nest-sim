using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	
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

		float newX = this.transform.position.x - mouseMove.x * dragScale;
		float newY = this.transform.position.y - mouseMove.y * dragScale;
		this.transform.position = new Vector3(newX, newY, this.transform.position.z);

		prevMousePos = Input.mousePosition;
	}

	public void DragEnd()
	{
	}
}
