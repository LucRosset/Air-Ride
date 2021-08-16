using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Accelerates machine orthogonally to the its normal.</summary>
public class Accelerate : MonoBehaviour, IAccelerate
{
	// CLASS VARIABLES

	[Tooltip("Default top speed for the machine, in m/s.")]
	[SerializeField] private float defaultTopSpeed = 10f;
	///<summary>Current top speed for the machine, in m/s.</summary>
	private float topSpeed;

	[Tooltip("Default forward acceleration for the machine, in m/s2.")]
	[SerializeField] private float defaultAcceleration = 10f;
	///<summary>Current forward acceleration for the machine, in m/s2.</summary>
	private float acceleration;

	[Tooltip("Default turn speed for the machine's velocity, in deg/s.")]
	[SerializeField] private float defaultTurnSpeed = 40f;
	///<summary>Current turn speed for the machine's velocity, in deg/s.</summary>
	private float turnSpeed;

	[Tooltip("Limit to drift, in degrees.")]
	[SerializeField] private float driftLimit = 30f;

	[Tooltip("Braking acceleration, in m/s2.")]
	[SerializeField] private float brakeAcceleration = 5f;
	[Tooltip("Downward velocity when braking airborne, in m/s.")]
	[SerializeField] private float fallSpeed = 20f;

	private Rigidbody myRigidbody = null;
	
	// INHERITED METHODS

	void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();

		topSpeed = defaultTopSpeed;
		acceleration = defaultAcceleration;
		turnSpeed = defaultTurnSpeed;
	}

	// CLASS METHODS

	public void AccelerateMachine(bool braking)
	{
		float linearAcceleration;
		Vector3 targetSpeed;
		Vector3 currentSpeedNormal = transform.up * Vector3.Dot(transform.up, myRigidbody.velocity);
		Vector3 currentSpeedTangential = myRigidbody.velocity - currentSpeedNormal;
		
		if (braking)
		{
			linearAcceleration = brakeAcceleration;
			targetSpeed = transform.forward * Mathf.Epsilon;
		}
		else
		{
			linearAcceleration = acceleration;
			targetSpeed = transform.forward * topSpeed;
		}

		if (Vector3.Angle(myRigidbody.velocity, targetSpeed) > driftLimit)
		{
			myRigidbody.velocity = Vector3.MoveTowards(
				currentSpeedTangential,
				targetSpeed,
				linearAcceleration * Time.fixedDeltaTime
			) + currentSpeedNormal;
		}
		else
		{
			myRigidbody.velocity = Vector3.RotateTowards(
				currentSpeedTangential,
				targetSpeed,
				turnSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime,
				linearAcceleration * Time.fixedDeltaTime
			) + currentSpeedNormal;
		}
	}
}
