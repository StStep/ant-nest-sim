using UnityEngine;
using System.Collections;

//TODO make manager an inherited class

/// <summary>
/// This class is a singleton used for game management.
/// </summary>
public class GameManager : MonoBehaviour {

	/// <summary>
	/// The single current instance of this class
	/// </summary>
	public static GameManager instance;

	/// <summary>
	/// The next ant ID that will be assigned.
	/// </summary>
	private static int nextAntID = 0;
	
	/// <summary>
	/// This function request a unique ant ID for tracking
	/// specific ants in the game.
	/// </summary>
	/// <returns>The next ant ID.</returns>
	public static int GetNextAntID() {
		return (nextAntID++);
	}
	
	void Awake()
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
	}
}
