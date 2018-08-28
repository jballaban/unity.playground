using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIActionProximityBase : AIActionBase
{
    public bool isInRange = false;
    public Vector3 destination = Vector3.zero;
    public EntityRecollection target = null;

    public override void Reset()
    {
        base.Reset();
        isInRange = false;
        target = null;
        destination = Vector3.zero;
    }

    public abstract void DetermineTarget(AIActionIAgent agent);

}
