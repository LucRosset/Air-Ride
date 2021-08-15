using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MachineSpeedMonitor : MonoBehaviour
{
	// CLASS VARIABLES
	
	[SerializeField] private MachineGroundMovement movement = null;

	private TextMeshProUGUI myText;

	// INHERITED METHODS

	void Start()
	{
		myText = GetComponent<TextMeshProUGUI>();
	}

	void Update()
	{
		myText.text = movement.GetVelocity().ToString();
	}

	// CLASS METHODS

}
