using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour, AIPlannerIAction
{
    public Dictionary<string, object> preConditions { get; set; }
    public Dictionary<string, object> effects { get; set; }
    public float cost { get; set; }
    public bool IsDone = false;

    protected virtual void Start()
    {
        cost = 1f;
        preConditions = new Dictionary<string, object>();
        effects = new Dictionary<string, object>();
    }

    public virtual void Reset()
    {
        IsDone = false;
    }

    public abstract bool Perform(AgentBase agent);

    protected bool Failure(AgentBase agent)
    {
        Debugger.Instance.Log<ActionBase>("Action Failed: " + this.GetType().Name, agent.gameObject);
        IsDone = true;
        return false;
    }

    protected bool Success(AgentBase agent)
    {
        Debugger.Instance.Log<ActionBase>("Action Success: " + this.GetType().Name, agent.gameObject);
        IsDone = true;
        //agent.WorldState.Apply(this.Effects);
        return true;
    }

}
