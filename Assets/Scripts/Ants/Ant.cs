using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This structure is used to represent an ant unit in the game.
/// </summary>
public class Ant
{
	public const float timeToMoveOneUnit = 1f; // Seconds to move 1 m
	public const float inverseMoveTime = 1f / timeToMoveOneUnit; // In m per second
	public const float carryCapacity = 5f;
	public const float foodEnegryCapacity = 1f;
	public const float minLifespan = 20f;
	public const float maxLifespan = 40f;
    public const float secPerAntUpdate = .5f;
    public const float energyUsageSec = .25f;
	public const float EnergyUsedPerAntUpdate = energyUsageSec*secPerAntUpdate;

	public Location destination;
	public Location origin;
	public float travelTimeLeft;
	public float foodCarrying;

	public Ant(Location origin, Location destination)
	{
		this.destination = destination;
		this.origin = origin;
		foodCarrying = 0f;
		travelTimeLeft = 0f;
	}

	public void Reset(Location origin, Location destination)
	{
		this.destination = destination;
		this.origin = origin;
		foodCarrying = 0f;
		travelTimeLeft = 0f;
	}
}

