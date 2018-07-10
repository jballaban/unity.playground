using System;
using System.Collections.Generic;
using UnityEngine;

public class Backpack
{
	State _state;
	Dictionary<Type, float> _resources = new Dictionary<Type, float>();
	public Backpack(State WorldState)
	{
		_state = WorldState;
	}

	public void Inc<TResource>(float amount) where TResource : ResourceBase
	{
		var key = typeof(TResource);
		if (!_resources.ContainsKey(key))
			_resources.Add(key, Mathf.Max(0f, amount));
		else
			_resources[key] = Mathf.Max(_resources[key] + amount, 0f);
		_state["has" + key.Name] = _resources[key] > 0f;
	}

	public float Take<TResource>(float amount)
	{
		var key = typeof(TResource);
		if (!_resources.ContainsKey(key))
			return 0f;
		var toremove = Mathf.Min(amount, _resources[key]);
		_resources[key] -= Mathf.Max(0f, amount);
		_state["has" + key.Name] = _resources[key] > 0f;
		return toremove;
	}

}