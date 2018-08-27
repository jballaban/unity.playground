using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIActionBase : MonoBehaviour, AIPlannerIAction
{
    public Dictionary<string, object> preConditions { get; set; }
    public Dictionary<string, object> effects { get; set; }
    public float cost { get; set; }
    public bool isDone = false;

    protected virtual void Start()
    {
        cost = 1f;
        preConditions = new Dictionary<string, object>();
        effects = new Dictionary<string, object>();
    }

    public virtual void Reset()
    {
        isDone = false;
    }

    public abstract bool Perform(AIActionIAgent agent);

    protected bool Failure(AIActionIAgent agent)
    {
        Debugger.Instance.Log<AIActionBase>("Action Failed: " + this.GetType().Name, agent.gameObject);
        isDone = true;
        return false;
    }

    protected bool Success(AIActionIAgent agent)
    {
        Debugger.Instance.Log<AIActionBase>("Action Success: " + this.GetType().Name, agent.gameObject);
        isDone = true;
        //agent.WorldState.Apply(this.Effects);
        return true;
    }

}
