using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
    {
        Grounded,
        Flying,
        Jumping,
        Gliding,
        Falling,
        Off
    }

///<summary>Controls in which state the machine is. This defines the machine's behavior and input scheeme.</summary>
[RequireComponent(typeof(MachineGroundMovement))]
public class MachineState : MonoBehaviour
{
	// CLASS VARIABLES

	[Tooltip("Machine's (starting) state.")]
    [SerializeField] private States state = States.Grounded;

	
	
	// INHERITED METHODS

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	// CLASS METHODS

}
