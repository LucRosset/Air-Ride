using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

///<summary>Class for a machine's movement control.</summary>
public class MachineGroundMovement : MonoBehaviour
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

	[Tooltip("Default turn speed for the machine, in deg/s. A larger Facing Turn Speed than a Turn Speed creates drift.")]
	[SerializeField] private float defaultFacingTurnSpeed = 50f;
	///<summary>Current turn speed for the machine, in deg/s. A larger Facing Turn Speed than a Turn Speed creates drift.</summary>
	private float facingTurnSpeed;

	[Tooltip("Limit to drift, in degrees.")]
	[SerializeField] private float driftLimit = 30f;

	[Tooltip("Default gravity acceleration, in m/s2.")]
	[SerializeField] private float defaultGravityAcceleration = 1f;
	///<summary>Current gravity acceleration, in m/s2.</summary>
	private float gravityAcceleration;

	[Tooltip("Braking acceleration, in m/s2.")]
	[SerializeField] private float brakeAcceleration = 5f;
	[Tooltip("Downward velocity when braking airborne, in m/s.")]
	[SerializeField] private float fallSpeed = 20f;

	[Tooltip("Default velocity right after boosting, in m/s.")]
	[SerializeField] private float defaultBoostVelocity = 40f;
	///<summary>Current velocity right after boosting, in m/s.</summary>
	private float boostVelocity;

	[Tooltip("Default charge rate, in %/s.")]
	[SerializeField] private float defaultChargeRate = .5f;
	///<summary>Current charge rate, in %/s.</summary>
	private float chargeRate;

	private MachineControls control;
	///<summary>x components refers to yaw, y component refers to pitch.</summary>
	private Vector2 yawPitch;
	private bool braking = false;
	private bool boosting = false;
	private float chargePercent = 0f;

	private Rigidbody myRigidbody = null;
	private Collider myCollider;

	// Provisory
	private Vector3 machineUp = Vector3.up;
	
	// INHERITED METHODS

	void Awake()
	{
		control = new MachineControls();
		control.Player.Move.performed += context => yawPitch = context.ReadValue<Vector2>();
		control.Player.Move.canceled += context => yawPitch = Vector2.zero;
		control.Player.Brake.performed += context => braking = context.ReadValueAsButton();
		control.Player.Brake.canceled += context => {
			braking = false;
			boosting = true;
		};
	}

	void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();
		myCollider = GetComponent<Collider>();

		topSpeed = defaultTopSpeed;
		acceleration = defaultAcceleration;
		turnSpeed = defaultTurnSpeed;
		facingTurnSpeed = defaultFacingTurnSpeed;
		gravityAcceleration = defaultGravityAcceleration;
		boostVelocity = defaultBoostVelocity;
		chargeRate = defaultChargeRate;
	}

	void FixedUpdate()
	{
		Pitch();
		Turn();
		if (boosting)
		{
			boosting = false;
			Boost();
		}
		else if (braking)
		{
			AccelerateTowards(transform.forward * Mathf.Epsilon, brakeAcceleration);
			ChargeBoost();
		}
		else
		{
			AccelerateTowards(transform.forward * topSpeed, acceleration);
		}
		GravityAccelerate();
	}

	void Update()
	{
		
	}

	void OnEnable()
	{
		control.Player.Enable();
	}

	void OnDisable()
	{
		control.Player.Disable();
	}

	void OnCollisionStay(Collision other)
	{
		
	}

	// CLASS METHODS

	private void Pitch()
	{

	}

	private void Turn()
	{
		transform.Rotate(Vector3.up * yawPitch.x * facingTurnSpeed * Time.fixedDeltaTime);
	}

	private void ChargeBoost()
	{
		chargePercent = Mathf.Clamp01(chargePercent + chargeRate * Time.fixedDeltaTime);
	}

	private void Boost()
	{
		myRigidbody.velocity = Vector3.ClampMagnitude(
			myRigidbody.velocity + transform.forward * boostVelocity * chargePercent * chargePercent,
			boostVelocity
		);
		chargePercent = 0f;
		boosting = false;
	}

	private void AccelerateTowards(Vector3 targetSpeed, float linearAcceleration)
	{
		if (Vector3.Angle(myRigidbody.velocity, targetSpeed) > driftLimit)
		{
			myRigidbody.velocity = Vector3.MoveTowards(
				myRigidbody.velocity,
				targetSpeed,
				linearAcceleration * Time.fixedDeltaTime
			);
		}
		else
		{
			myRigidbody.velocity = Vector3.RotateTowards(
				myRigidbody.velocity,
				targetSpeed,
				turnSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime,
				linearAcceleration * Time.fixedDeltaTime
			);
		}
	}

	private void GravityAccelerate()
	{
		myRigidbody.velocity += Vector3.down * gravityAcceleration * Time.fixedDeltaTime;
	}

	// UI

	public float GetVelocity() { return myRigidbody.velocity.magnitude; }

	public float GetCurrentCharge() { return chargePercent; }

	// DEBUG

	void OnDrawGizmosSelected()
	{
		if (myRigidbody != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + myRigidbody.velocity);
		}
	}
}
