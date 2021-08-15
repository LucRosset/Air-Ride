using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
	// CLASS VARIABLES

	[Tooltip("Default turn speed for the machine, in deg/s. A larger Facing Turn Speed than a Turn Speed creates drift.")]
	[SerializeField] private float defaultFacingTurnSpeed = 50f;
	///<summary>Current turn speed for the machine, in deg/s. A larger Facing Turn Speed than a Turn Speed creates drift.</summary>
	private float facingTurnSpeed;
	
	// INHERITED METHODS

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	// CLASS METHODS

	public void YawTurn(float yaw)
	{
		transform.Rotate(Vector3.up * yaw * facingTurnSpeed * Time.fixedDeltaTime);
	}
}
