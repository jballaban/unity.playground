using System;
using System.Collections.Generic;
using System.Linq;
using API.Memory.Contract;
using API.Memory.Internal;
using UnityEngine;
using UnityEngine.Events;

namespace API.Memory
{
    public class ForgetMemoryEvent : UnityEvent<IMemory> { }

    public class MemoryComponent : MonoBehaviour
    {
        Dictionary<MemoryID, IMemory> memoryById = new Dictionary<MemoryID, IMemory>();
        public ForgetMemoryEvent ForgetEvent;

        public T Recall<T>(MemoryID id, IMemory def = null) where T : IMemory
        {
            if (!memoryById.ContainsKey(id)) return (T)def;
            return (T)memoryById[id];
        }

        public virtual void Remember(IMemory memory)
        {
            memoryById[memory.id] = memory; // update memoryid
        }

        public virtual void Forget(IMemory memory)
        {
            if (!memoryById.ContainsKey(memory.id)) return;
            memoryById.Remove(memory.id);
            ForgetEvent.Invoke(memory);
        }
    }
}