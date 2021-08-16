using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour, IBoost
{
	// CLASS VARIABLES

	[Tooltip("Default charge rate, in %/s.")]
	[SerializeField] private float defaultChargeRate = .5f;
	///<summary>Current charge rate, in %/s.</summary>
	private float chargeRate;

	[Tooltip("Default velocity right after boosting, in m/s.")]
	[SerializeField] private float defaultBoostVelocity = 40f;
	///<summary>Current velocity right after boosting, in m/s.</summary>
	private float boostVelocity;
	
	private Rigidbody myRigidbody = null;
	public float charge {get; private set;} = 0f;

	// INHERITED METHODS

	void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();

		chargeRate = defaultChargeRate;
		boostVelocity = defaultBoostVelocity;
	}

	// CLASS METHODS

	public void ChargeBoost()
	{
		charge = Mathf.Clamp01(charge + chargeRate * Time.fixedDeltaTime);
	}

	public void StartBoost()
	{
		Vector3 currentSpeedNormal = transform.up * Vector3.Dot(transform.up, myRigidbody.velocity);
		Vector3 currentSpeedTangential = myRigidbody.velocity - currentSpeedNormal;
		myRigidbody.velocity = Vector3.ClampMagnitude(
			currentSpeedTangential + transform.forward * boostVelocity * charge * charge,
			boostVelocity
		) + currentSpeedNormal;
		charge = 0f;
	}
}
