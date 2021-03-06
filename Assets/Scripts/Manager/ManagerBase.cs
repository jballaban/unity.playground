using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ManagerBase
{
    ManagerBase() { }

    public Dictionary<Type, IResourceManager> _resources = new Dictionary<Type, IResourceManager>();

    public T GetManager<T>() where T : IResourceManager
    {
        return (T)_resources[typeof(T)];
    }

    public IResourceManager GetManager(Type t)
    {
        if (!_resources.ContainsKey(t))
            throw new Exception("Couldn't find manager for " + t.Name);
        return _resources[t];
    }

    static ManagerBase _instance = null;
    public static ManagerBase Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ManagerBase();
            return _instance;
        }
    }

    public void Register(IResourceManager manager)
    {
        _resources.Add(manager.GetType(), manager);
    }
}