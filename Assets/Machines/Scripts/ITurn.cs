using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurn
{
	///<summary>Turns machine's facing in the horizontal plane.</summary>
	///<param name="yaw">Facing turn, in degrees per second.</param>
	void YawTurn(float yaw);
}
