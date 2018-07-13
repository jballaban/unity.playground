using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MemoryBase : MonoBehaviour
{
	State _state;
	Dictionary<string, List<Recollection>> Facts = new Dictionary<string, List<Recollection>>();

	public void Initialize(State state)
	{
		_state = state;
	}

	public List<T> GetAll<T>(Type type) where T : Recollection
	{
		var key = type.Name;
		if (Facts.ContainsKey(key))
			return Facts[key].Cast<T>().ToList();
		return new List<T>();
	}

	public virtual Recollection Remember(ResourceBase fact)
	{
		var key = fact.GetType().Name;
		var recollection = new ResourceBaseRecollection(fact);
		if (!Facts.ContainsKey(key))
			Facts[key] = new List<Recollection>() { recollection };
		else
		{
			var exists = Facts[key].FirstOrDefault(f => f.InstanceID == recollection.InstanceID);
			if (exists != null)
			{
				(exists as ResourceBaseRecollection).Refresh(fact);
				return exists;
			}
			Facts[key].Add(recollection);
		}
		_state["know" + key] = true;
		Debug.Log("Memory.Remember: " + key);
		return recollection;
	}

	public virtual void Forget(Recollection fact)
	{
		var key = fact.Type.Name;
		if (!Facts.ContainsKey(key))
			return;
		Facts[key].Remove(fact);
		_state["know" + key] = Facts[key].Count > 0;
		Debug.Log("Memory.Forget: " + key);
	}
}
