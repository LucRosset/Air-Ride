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
	[Tooltip("Current top speed for the machine, in m/s.")]
	public float topSpeed;

	[Tooltip("Default forward acceleration for the machine, in m/s2.")]
	[SerializeField] private float defaultAcceleration = 10f;
	[Tooltip("Current forward acceleration for the machine, in m/s2.")]
	public float acceleration;

	[Tooltip("Default turn speed for the machine's velocity, in deg/s.")]
	[SerializeField] private float defaultTurnSpeed = 40f;
	[Tooltip("Current turn speed for the machine's velocity, in deg/s.")]
	public float turnSpeed;

	[Tooltip("Default turn speed for the machine, in deg/s. A larger Facing Turn Speed than a Turn Speed creates drift.")]
	[SerializeField] private float defaultFacingTurnSpeed = 50f;
	[Tooltip("Current turn speed for the machine, in deg/s. A larger Facing Turn Speed than a Turn Speed creates drift.")]
	public float facingTurnSpeed;

	[Tooltip("Breaking acceleration, in m/s2.")]
	[SerializeField] private float breakAcceleration = 5f;
	[Tooltip("Downward velocity when breaking airborne, in m/s.")]
	[SerializeField] private float fallSpeed = 20f;

	private MachineControls control;
	///<summary>x components refers to yaw, y component refers to pitch.</summary>
	private Vector2 yawPitch;
	private bool breaking;

	private Rigidbody myRigidbody;
	private Collider myCollider;

	// Provisory
	private Vector3 machineUp = Vector3.up;
	
	// INHERITED METHODS

	void Awake()
	{
		control = new MachineControls();
		control.Player.Move.performed += context => yawPitch = context.ReadValue<Vector2>();
		control.Player.Move.canceled += context => yawPitch = Vector2.zero;
		control.Player.Break.performed += context => breaking = context.ReadValueAsButton();
		control.Player.Break.canceled += context => breaking = false;
	}

	void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();
		myCollider = GetComponent<Collider>();

		topSpeed = defaultTopSpeed;
		acceleration = defaultAcceleration;
		turnSpeed = defaultTurnSpeed;
		facingTurnSpeed = defaultFacingTurnSpeed;
	}

	void FixedUpdate()
	{
		Pitch();
		Turn();
		if (breaking)
			AccelerateTowards(Vector3.zero, breakAcceleration);
		else
			AccelerateTowards(transform.forward * topSpeed, acceleration);
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

	private void AccelerateTowards(Vector3 targetSpeed, float linearAcceleration)
	{
		myRigidbody.velocity = Vector3.RotateTowards(
			myRigidbody.velocity,
			targetSpeed,
			turnSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime,
			linearAcceleration * Time.fixedDeltaTime
		);
	}

	public float GetVelocity() { return myRigidbody.velocity.magnitude; }
}
