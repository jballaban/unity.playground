using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Memory
{
    State _state;
    Dictionary<string, List<Recollection>> Facts = new Dictionary<string, List<Recollection>>();

    public Memory(State state)
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

    public Recollection Remember(ResourceBase fact)
    {
        var key = fact.GetType().Name;
        var recollection = new ResourceBaseRecollection(fact);
        if (!Facts.ContainsKey(key))
            Facts[key] = new List<Recollection>() { recollection };
        else
            Facts[key].Add(recollection);
        _state["know" + key] = true;
        return recollection;
    }

    public void Forget(Recollection fact)
    {
        var key = fact.Type.Name;
        if (!Facts.ContainsKey(key))
            return;
        Facts[key].Remove(fact);
        _state["know" + key] = Facts[key].Count > 0;
    }
}

public abstract class Recollection
{
    public Type Type;
}

public class ResourceBaseRecollection : Recollection
{
    public ResourceBaseRecollection(ResourceBase resource)
    {
        Quantity = resource.Quantity;
        Position = resource.transform.position;
        Scale = resource.transform.localScale;
        Rotation = resource.transform.rotation;
        InstanceID = resource.gameObject.GetInstanceID();
        manager = resource.Manager;
        Type = resource.GetType();
    }
    public readonly float Quantity;
    public readonly Vector3 Position;
    public readonly Vector3 Scale;
    public readonly Quaternion Rotation;
    public readonly int InstanceID;
    ResourceManager manager;
    public ResourceBase GetOriginal()
    {
        return manager.FindByID(InstanceID);
    }
}