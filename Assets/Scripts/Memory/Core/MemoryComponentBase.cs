using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MemoryComponentBase : ComponentBase
{
    MemorySystem _memorySystem;
    protected AgentBase agent;
    void Awake()
    {
        _memorySystem = GetComponentInParent<MemorySystem>();
        if (_memorySystem == null)
            throw new Exception("MemorySystem required to be a parent of MemoryComponent");
        _memorySystem.RegisterEvent(new MemorySystem.RememberEvent());
        _memorySystem.RegisterEvent(new MemorySystem.ForgetEvent());
        agent = GetComponent<AgentBase>();
    }

    Dictionary<int, object> _memories = new Dictionary<int, object>();

    public virtual void Remember<T>(int id, object data) where T : IRecollection
    {
        if (_memories.ContainsKey(id))
        {
            ((T)_memories[id]).Refresh(data);
            return;
        }
        var safedata = Activator.CreateInstance(typeof(T), new object[] { id, data });
        _memories.Add(id, safedata);
        _memorySystem.GetEvent<MemorySystem.RememberEvent>().Invoke(id, safedata);
    }

    public virtual void Forget(int id)
    {
        if (_memories.ContainsKey(id))
        {
            var data = _memories[id];
            _memories.Remove(id);
            _memorySystem.GetEvent<MemorySystem.ForgetEvent>().Invoke(id, data);
        }
    }

    protected T Get<T>(int id)
    {
        return (T)(_memories.ContainsKey(id) ? _memories[id] : null);
    }

    public T Recall<T>(int id)
    {
        return Get<T>(id);
    }

}