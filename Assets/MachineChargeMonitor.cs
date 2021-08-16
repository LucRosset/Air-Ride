using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineChargeMonitor : MonoBehaviour
{
	// CLASS VARIABLES

	[SerializeField] private Boost boost = null;

	private Slider slider;
	
	// INHERITED METHODS

	void Start()
	{
		slider = GetComponent<Slider>();
	}

	void Update()
	{
		slider.value = boost.charge;
	}

	// CLASS METHODS

}
