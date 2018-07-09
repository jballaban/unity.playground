using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Memory
{
	State _state;
	Dictionary<string, List<object>> Facts = new Dictionary<string, List<object>>();
	public Memory(State state)
	{
		_state = state;
	}

	public List<TFact> GetAll<TFact>(string key = null)
	{
		if (key == null) key = typeof(TFact).Name;
		if (Facts.ContainsKey(key))
			return Facts[key].Cast<TFact>().ToList();
		return new List<TFact>();
	}

	public void Remember(object fact, string key = null)
	{
		if (key == null) key = fact.GetType().Name;
		if (!Facts.ContainsKey(key))
			Facts[key] = new List<object>() { fact };
		else
			Facts[key].Add(fact);
		_state["know" + key] = true;
	}

	public void Forget(object fact, string key = null)
	{
		if (key == null) key = fact.GetType().Name;
		if (!Facts.ContainsKey(key))
			return;
		Facts[key].Remove(fact);
		_state["know" + key] = Facts[key].Count > 0;
	}
}