using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class OnFootController : MonoBehaviour
{
	// CLASS VARIABLES

	[Tooltip("Transform of the camera that follows the player.")]
	[SerializeField] private Transform cameraTransform = null;

	[Tooltip("Movement acceletarion, in m/s2.")]
	[SerializeField] private float acceleration = 1f;

	private Rigidbody myRigidbody;
	private Vector3 movement;
	private float movementX;
	private float movementY;
	
	// INHERITED METHODS

	void Start()
	{
		myRigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		CalculateMovementDirection();
		myRigidbody.AddForce(movement * acceleration * Time.fixedDeltaTime);

		ChangeFacing();
	}

	private void OnMove(InputValue movementValue)
	{
		Vector2 movementVector = movementValue.Get<Vector2>();
		movementX = movementVector.x;
		movementY = movementVector.y;
	}

	// CLASS METHODS

	private void CalculateMovementDirection()
	{
		movement = cameraTransform.localToWorldMatrix.MultiplyVector(new Vector3(
			movementX,
			0f,
			movementY
		));
		movement.y = 0f;
		movement.Normalize();
	}

	private void ChangeFacing()
	{
		// if (movement.magnitude > Mathf.Epsilon)
			transform.rotation = Quaternion.LookRotation(movement, Vector3.up);
	}

}
