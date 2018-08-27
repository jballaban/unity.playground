using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProximityActionBase : AIActionBase
{
    public bool IsInRange = false;
    public Vector3 Destination = Vector3.zero;
    public EntityRecollection Target = null;

    public override void Reset()
    {
        base.Reset();
        IsInRange = false;
        Target = null;
        Destination = Vector3.zero;
    }

    public abstract void DetermineTarget(AgentBase agent);

}
