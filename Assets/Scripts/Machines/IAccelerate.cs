using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAccelerate
{
	///<summary>Change velocity towards a target.</summary>
	///<param name="braking">If braking is true, will use the braking magnitude as target.</param>
	void AccelerateMachine(bool braking);
}
