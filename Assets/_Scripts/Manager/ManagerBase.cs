using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(DepotManager), typeof(TreeManager), typeof(LogManager))]
public class ManagerBase : MonoBehaviour
{
    public Dictionary<Type, ResourceManager> _resources = new Dictionary<Type, ResourceManager>();

    public T GetManager<T>() where T : ResourceManager
    {
        return (T)_resources[typeof(T)];
    }

    static ManagerBase _instance = null;
    public static ManagerBase Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ManagerBase>();
            return _instance;
        }
    }

    public void Register(ResourceManager manager)
    {
        _resources.Add(manager.GetType(), manager);
    }
}