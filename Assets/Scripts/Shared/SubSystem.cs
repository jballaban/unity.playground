using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SubSystem : MonoBehaviour
{
    List<UnityEventBase> _events = new List<UnityEventBase>();

    public void RegisterEvent(UnityEventBase e)
    {
        if (!_events.Contains(e))
            _events.Add(e);
    }

    public T GetEvent<T>() where T : UnityEventBase
    {
        var result = _events.FirstOrDefault(e => e is T);
        if (result == null)
            throw new Exception(String.Format("Attempted to retrieve unknown event {0} on {1}", typeof(T).Name, this.GetType().Name));
        return (T)result;
    }

}