using UnityEngine;
using System.Collections;

public class AntParticleController : MonoBehaviour {

	public ParticleSystem antPartsA1;
	public ParticleSystem antPartsA2;
	public ParticleSystem antPartsA3;
	public ParticleSystem antPartsA4;

	public ParticleSystem antPartsB1;
	public ParticleSystem antPartsB2;
	public ParticleSystem antPartsB3;
	public ParticleSystem antPartsB4;

	protected float _percTrafficOnSideA;
	protected float _percUseOfCapacity;

	protected float _percDensity;
	public float PercDensity
	{
		set
		{
			_percDensity = value;
		}
	}
	protected float _lengthOfPath;
	public float LengthOfPath
	{
		set
		{
			_lengthOfPath = value;
		}
	}

	protected void Awake()
	{
		StopAll();

		_percTrafficOnSideA = 0;
		_percUseOfCapacity = 0;
		_percDensity = 0;
		_lengthOfPath = 0;
	}

	// Use this for initialization
	protected void Start() 
	{

	}
	
	// Update is called once per frame
	protected void Update() 
	{
	
	}

	public void SetPathStatus(float percUseCap, float percATraffic)
	{
		_percTrafficOnSideA = percATraffic;
		_percUseOfCapacity = percUseCap;

		if(_percUseOfCapacity != 0)
		{
			PlayBasedOnStatus();
		}
		else
		{
			StopAll();
		}
	}

	protected void PlayBasedOnStatus()
	{
		int emitterEnableCount;
		if(_percUseOfCapacity < .25)
		{
			emitterEnableCount = 1;
		}
		else if(_percUseOfCapacity < .5)
		{
			emitterEnableCount = 2;
		}
		else if(_percUseOfCapacity < .75)
		{
			emitterEnableCount = 3;
		}
		else
		{
			emitterEnableCount = 4;
		}

		int emitterACount;
		int emitterBCount;
		if(_percTrafficOnSideA < float.Epsilon)
		{
			emitterACount = 0;
			emitterBCount = emitterEnableCount;
		}
		if(_percTrafficOnSideA < .25)
		{
			if(emitterEnableCount == 1)
			{
				emitterACount = 1;
				emitterBCount = 1;
			}
			else if(emitterEnableCount == 2)
			{
				emitterACount = 1;
				emitterBCount = 1;
			}
			else if(emitterEnableCount == 3)
			{
				emitterACount = 1;
				emitterBCount = 2;
			}
			else
			{
				emitterACount = 1;
				emitterBCount = 3;
			}
		}
		else if(_percTrafficOnSideA < .5)
		{
			if(emitterEnableCount == 1)
			{
				emitterACount = 1;
				emitterBCount = 1;
			}
			else if(emitterEnableCount == 2)
			{
				emitterACount = 1;
				emitterBCount = 1;
			}
			else if(emitterEnableCount == 3)
			{
				emitterACount = 2;
				emitterBCount = 2;
			}
			else
			{
				emitterACount = 2;
				emitterBCount = 2;
			}
		}
		else if(_percTrafficOnSideA < .75)
		{
			if(emitterEnableCount == 1)
			{
				emitterACount = 1;
				emitterBCount = 1;
			}
			else if(emitterEnableCount == 2)
			{
				emitterACount = 1;
				emitterBCount = 1;
			}
			else if(emitterEnableCount == 3)
			{
				emitterACount = 2;
				emitterBCount = 1;
			}
			else
			{
				emitterACount = 3;
				emitterBCount = 1;
			}
		}
		else
		{
			emitterACount = emitterEnableCount;
			emitterBCount = 0;
		}

		CompareAtoBForPlay(emitterACount, emitterBCount);
	}

	protected void CompareAtoBForPlay(int cntToRunA, int cntToRunB)
	{
		bool outerRightA = false;
		bool innerRightA = false;
		bool innerLeftA = false;
		bool outerLeftA = false;

		bool outerRightB = false;
		bool innerRightB = false;
		bool innerLeftB = false;
		bool outerLeftB = false;

		//  A one
		if(cntToRunA == 1 && cntToRunB == 0)
		{
			innerLeftA = true;
		}
		// A two
		else if(cntToRunA == 2 && cntToRunB == 0)
		{
			innerLeftA = true;
			outerLeftA = true;
		}
		// A two b one
		else if(cntToRunA == 2 && cntToRunB == 1)
		{
			innerLeftA = true;
			outerLeftA = true;
			innerLeftB = true;
		}
		// A three b one
		else if(cntToRunA == 3 && cntToRunB == 1)
		{
			innerLeftA = true;
			outerLeftA = true;
			innerRightA = true;
			outerLeftB = true;
		}
		// A and B both one
		else if(cntToRunA == 1 && cntToRunB == 1)
		{
			innerLeftA = true;
			innerLeftB = true;
		}
		// A two B two
		else if(cntToRunA == 2 && cntToRunB == 2)
		{
			innerLeftA = true;
			outerLeftA = true;
			innerLeftB = true;
			outerLeftB = true;
		}
		// A one B three
		else if(cntToRunA == 1 && cntToRunB == 3)
		{
			outerLeftA = true;
			innerRightB = true;
			innerLeftB = true;
			outerLeftB = true;
		}
		// A one B two
		else if(cntToRunA == 1 && cntToRunB == 2)
		{
			innerLeftA = true;
			innerLeftB = true;
			outerLeftB = true;
		}
		// A two
		else if(cntToRunA == 0 && cntToRunB == 2)
		{
			innerLeftB = true;
			outerLeftB = true;
		}
		// B One
		else if(cntToRunA == 0 && cntToRunB == 1)
		{
			innerLeftB = true;
		}
		// Unhandled
		else
		{
			Debug.Log("CompareAtoBForPlay: Warning, not handling state with A cnt " + cntToRunA + " and B cnt " + cntToRunB);
		}

		// Stop/Play emitters
		PlayA(outerLeftA,innerLeftA,innerRightA,outerRightA);
		PlayB(outerLeftB,innerLeftB,innerRightB,outerRightB);
	}

	protected void PlayA(bool outerLeft, bool innerLeft, bool innerRight, bool outerRight)
	{
		if(outerRight)
		{
			if(!antPartsA1.isPlaying)
			{
				antPartsA1.Play();
			}
		}
		else
		{
			if(antPartsA1.isPlaying)
			{
				antPartsA1.Stop();
			}
		}

		if(innerRight)
		{
			if(!antPartsA2.isPlaying)
			{
				antPartsA2.Play();
			}
		}
		else
		{
			if(antPartsA2.isPlaying)
			{
				antPartsA2.Stop();
			}
		}

		if(innerLeft)
		{
			if(!antPartsA3.isPlaying)
			{
				antPartsA3.Play();
			}
		}
		else
		{
			if(antPartsA3.isPlaying)
			{
				antPartsA3.Stop();
			}
		}

		if(outerLeft)
		{
			if(!antPartsA4.isPlaying)
			{
				antPartsA4.Play();
			}
		}
		else
		{
			if(antPartsA4.isPlaying)
			{
				antPartsA4.Stop();
			}
		}
	}

	protected void PlayB(bool outerLeft, bool innerLeft, bool innerRight, bool outerRight)
	{
		if(outerRight)
		{
			if(!antPartsB1.isPlaying)
			{
				antPartsB1.Play();
			}
		}
		else
		{
			if(antPartsB1.isPlaying)
			{
				antPartsB1.Stop();
			}
		}
		
		if(innerRight)
		{
			if(!antPartsB2.isPlaying)
			{
				antPartsB2.Play();
			}
		}
		else
		{
			if(antPartsB2.isPlaying)
			{
				antPartsB2.Stop();
			}
		}
		
		if(innerLeft)
		{
			if(!antPartsB3.isPlaying)
			{
				antPartsB3.Play();
			}
		}
		else
		{
			if(antPartsB3.isPlaying)
			{
				antPartsB3.Stop();
			}
		}
		
		if(outerLeft)
		{
			if(!antPartsB4.isPlaying)
			{
				antPartsB4.Play();
			}
		}
		else
		{
			if(antPartsB4.isPlaying)
			{
				antPartsB4.Stop();
			}
		}
	}

	protected void StopAll()
	{
		if(antPartsA1.isPlaying)
		{
			antPartsA1.Stop();
		}
		if(antPartsA2.isPlaying)
		{
			antPartsA2.Stop();
		}
		if(antPartsA3.isPlaying)
		{
			antPartsA3.Stop();
		}
		if(antPartsA4.isPlaying)
		{
			antPartsA4.Stop();
		}

		if(antPartsB1.isPlaying)
		{
			antPartsB1.Stop();
		}
		if(antPartsB2.isPlaying)
		{
			antPartsB2.Stop();
		}
		if(antPartsB3.isPlaying)
		{
			antPartsB3.Stop();
		}
		if(antPartsB4.isPlaying)
		{
			antPartsB4.Stop();
		}
	}
}
