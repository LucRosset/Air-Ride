using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoost
{
	///<summary>Charge the boost for this fixed update.</summary>
	void ChargeBoost();

	///<summary>Start the boost procedure.</summary>
	void StartBoost();
}
