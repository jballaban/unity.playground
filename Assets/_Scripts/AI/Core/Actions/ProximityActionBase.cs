using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProximityActionBase : ActionBase
{
	public bool IsInRange = false;
	public Transform Target = null;

	public override void Reset()
	{
		IsInRange = false;
		Target = null;
	}

	public abstract void DetermineTarget(AgentBase agent);

}
