using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SensorySystem))]
public class ResourceMemoryComponent : MemoryComponentBase
{

    Dictionary<Type, List<int>> _resourceIds = new Dictionary<Type, List<int>>();

    void Start()
    {
        GetComponent<SensorySystem>().GetEvent<SensorySystem.ObserveEnterEvent>().AddListener(OnObserveEnter);
    }

    public List<T> GetAll<T>()
    {
        if (!_resourceIds.ContainsKey(typeof(T)))
            return new List<T>();
        return _resourceIds[typeof(T)].Select(r => Recall<T>(r)).ToList();
    }

    void OnObserveEnter(GameObject obj)
    {
        if (obj.CompareTag("interactive"))
        {
            var resource = obj.GetComponent<ResourceBase>();
            if (resource == null) return;
            var id = resource.GetInstanceID();
            Remember<ResourceRecollection>(id, resource);
            var type = resource.GetType();
            if (!_resourceIds.ContainsKey(type))
                _resourceIds.Add(type, new List<int>());
            if (!_resourceIds[type].Contains(id))
                _resourceIds[type].Add(id);
        }
    }
}