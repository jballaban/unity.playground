using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
    public Dictionary<string, object> PreConditions;
    public Dictionary<string, object> Effects;
    public bool RequiresInRange = false;
    public bool IsInRange = false;
    public GameObject Target = null;
    public float Cost = 0;

    public ActionBase()
    {
        PreConditions = new Dictionary<string, object>();
        Effects = new Dictionary<string, object>();
    }

    public void Reset()
    {
        IsInRange = false;
        Target = null;
    }

    public abstract bool IsDone();

    public abstract bool Perform(GameObject actor);
    public abstract bool IsValid(GameObject actor);

}
