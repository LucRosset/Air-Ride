using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineChargeMonitor : MonoBehaviour
{
	// CLASS VARIABLES

	[SerializeField] private MachineGroundMovement movement = null;

	private Slider slider;
	
	// INHERITED METHODS

	void Start()
	{
		slider = GetComponent<Slider>();
	}

	void Update()
	{
		slider.value = movement.GetCurrentCharge();
	}

	// CLASS METHODS

}
