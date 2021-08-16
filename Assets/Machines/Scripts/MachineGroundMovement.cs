using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

///<summary>Class for a machine's movement control.</summary>
public class MachineGroundMovement : MonoBehaviour
{
	// CLASS VARIABLES

	[Tooltip("Default gravity acceleration, in m/s2.")]
	[SerializeField] private float defaultGravityAcceleration = 1f;
	///<summary>Current gravity acceleration, in m/s2.</summary>
	private float gravityAcceleration;

	private IAccelerate accelerate;
	private IBoost boost;
	private ITurn turn;

	private MachineControls control;
	///<summary>x components refers to yaw, y component refers to pitch.</summary>
	private Vector2 yawPitch;
	private bool braking = false;
	private bool boosting = false;

	private Rigidbody myRigidbody = null;

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

		accelerate = GetComponent<IAccelerate>();
		boost = GetComponent<IBoost>();
		turn = GetComponent<ITurn>();
	}

	void FixedUpdate()
	{
		// Pitch();
		turn.YawTurn(yawPitch.x);
		if (braking)
			boost.ChargeBoost();
		if (boosting)
		{
			boosting = false;
			boost.StartBoost();
		}
		accelerate.AccelerateMachine(braking);
		GravityAccelerate();
	}

	void OnEnable() { control.Player.Enable(); }

	void OnDisable() { control.Player.Disable(); }

	// CLASS METHODS

	private void GravityAccelerate()
	{
		myRigidbody.velocity += Vector3.down * gravityAcceleration * Time.fixedDeltaTime;
	}

	// UI

	public float GetVelocity() { return myRigidbody.velocity.magnitude; }

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
