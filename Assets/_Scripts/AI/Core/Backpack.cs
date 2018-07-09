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
		if (!_resources.ContainsKey(typeof(TResource)))
			_resources.Add(typeof(TResource), Mathf.Max(0f, amount));
		else
			_resources[typeof(TResource)] = Mathf.Max(_resources[typeof(TResource)] + amount, 0f);
		_state["has" + typeof(TResource).Name] = _resources[typeof(TResource)] > 0f;
	}

}