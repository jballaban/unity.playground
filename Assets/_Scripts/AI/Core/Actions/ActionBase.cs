using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBase : MonoBehaviour
{
	public Dictionary<string, object> PreConditions;
	public Dictionary<string, object> Effects;
	public float Cost = 1f;
	public bool IsDone = false;

	protected virtual void Start()
	{
		PreConditions = new Dictionary<string, object>();
		Effects = new Dictionary<string, object>();
	}

	public virtual void Reset()
	{
		IsDone = false;
	}

	public abstract bool Perform(AgentBase agent);

	protected bool Failure(AgentBase agent)
	{
		Debug.Log("Action Failed: " + this.GetType().Name);
		IsDone = true;
		return false;
	}

	protected bool Success(AgentBase agent)
	{
		Debug.Log("Action Success: " + this.GetType().Name);
		IsDone = true;
		//agent.WorldState.Apply(this.Effects);
		return true;
	}

}
