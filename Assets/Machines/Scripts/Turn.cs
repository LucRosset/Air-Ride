using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Turns machine over its normal.</summary>
public class Turn : MonoBehaviour, ITurn
{
	// CLASS VARIABLES

	[Tooltip("Default turn speed for the machine, in deg/s. A larger Facing Turn Speed than a Turn Speed creates drift.")]
	[SerializeField] private float defaultFacingTurnSpeed = 50f;
	///<summary>Current turn speed for the machine, in deg/s. A larger Facing Turn Speed than a Turn Speed creates drift.</summary>
	private float facingTurnSpeed;
	
	// INHERITED METHODS

	void Start()
	{
		facingTurnSpeed = defaultFacingTurnSpeed;
	}

	// CLASS METHODS

	public void YawTurn(float yaw)
	{
		transform.Rotate(Vector3.up * yaw * facingTurnSpeed * Time.fixedDeltaTime);
	}
}
