using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProximityActionBase : ActionBase
{
	public bool IsInRange = false;

	public ResourceBaseRecollection Target = null;

	public override void Reset()
	{
		base.Reset();
		IsInRange = false;
		Target = null;
	}

	public abstract void DetermineTarget(AgentBase agent);

}
