using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
	public Dictionary<string, object> PreConditions;
	public Dictionary<string, object> Effects;
	public float Cost = 0;
	public bool IsDone = false;

	public ActionBase()
	{
		PreConditions = new Dictionary<string, object>();
		Effects = new Dictionary<string, object>();
	}

	public virtual void Reset()
	{ }

	public virtual bool Perform(AgentBase agent)
	{
		agent.WorldState.Apply(this.Effects);
		return true;
	}

}
