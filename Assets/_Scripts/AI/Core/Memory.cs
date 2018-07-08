using System;
using System.Collections.Generic;
using UnityEngine;

public class Memory
{
    State _state;
    Dictionary<Type, List<GameObject>> Resources = new Dictionary<Type, List<GameObject>>();
    public Memory(State state)
    {
        _state = state;
    }

    public void Remember<TResource>(GameObject resource)
    {
        if (!Resources.ContainsKey(typeof(TResource)))
            Resources[typeof(TResource)] = new List<GameObject>() { resource };
        else
            Resources[typeof(TResource)].Add(resource);
        _state["know" + typeof(TResource).Name] = true;
    }

    public void Forget<TResource>(GameObject resource)
    {
        if (!Resources.ContainsKey(typeof(TResource)))
            return;
        Resources[typeof(TResource)].Remove(resource);
        _state["know" + typeof(TResource).Name] = Resources[typeof(TResource)].Count > 0;
    }
}