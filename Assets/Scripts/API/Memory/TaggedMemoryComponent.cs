using System;
using System.Collections.Generic;
using System.Linq;
using API.Memory.Contract;
using API.Memory.Internal;
using UnityEngine;

namespace API.Memory
{
    [RequireComponent(typeof(MemoryComponent))]
    public class TaggedMemoryComponent : MonoBehaviour
    {
        Dictionary<string, HashSet<MemoryID>> memoryIdsByTag = new Dictionary<string, HashSet<MemoryID>>();
        Dictionary<MemoryID, HashSet<string>> tagsByMemoryId = new Dictionary<MemoryID, HashSet<string>>();
        MemoryComponent _memory;

        void Awake()
        {
            _memory = GetComponent<MemoryComponent>();
            _memory.ForgetEvent.AddListener(Forget);
        }

        public void Remember(IMemory memory, params string[] tags)
        {
            _memory.Remember(memory);
            for (int i = 0; i < tags.Length; i++)
            {
                if (!memoryIdsByTag.ContainsKey(tags[i]))
                    memoryIdsByTag[tags[i]] = new HashSet<MemoryID>() { memory.id };
                else
                    memoryIdsByTag[tags[i]].Add(memory.id);
                if (!tagsByMemoryId.ContainsKey(memory.id))
                    tagsByMemoryId[memory.id] = new HashSet<string>() { tags[i] };
                else
                    tagsByMemoryId[memory.id].Add(tags[i]);
            }
        }

        public void Forget(IMemory memory, params string[] tags)
        {
            for (int i = 0; i < tags.Length; i++)
            {
                if (memoryIdsByTag.ContainsKey(tags[i]))
                    memoryIdsByTag[tags[i]].Remove(memory.id);
                if (tagsByMemoryId.ContainsKey(memory.id))
                    tagsByMemoryId[memory.id].Remove(tags[i]);
            }
            if (tagsByMemoryId.ContainsKey(memory.id) && tagsByMemoryId[memory.id].Count == 0)
                tagsByMemoryId.Remove(memory.id);
        }

        public List<T> Recall<T>(params string[] tags) where T : IMemory
        {
            IEnumerable<T> result = null;
            for (int i = 0; i < tags.Length; i++)
            {
                if (!memoryIdsByTag.ContainsKey(tag)) continue;
                if (result == null)
                    result = memoryIdsByTag[tag].Select(x => _memory.Recall<T>(x));
                else
                    result = result.Intersect(memoryIdsByTag[tag].Select(x => _memory.Recall<T>(x)));
            }
            return result == null ? new List<T>() : result.ToList();
        }

        void Forget(IMemory memory)
        {
            Forget(memory, tagsByMemoryId[memory.id].ToArray());
        }

    }
}