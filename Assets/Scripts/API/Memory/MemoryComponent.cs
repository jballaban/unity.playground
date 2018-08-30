using System;
using System.Collections.Generic;
using System.Linq;
using API.Memory.Contract;
using UnityEngine;

namespace API.Memory
{
    public class MemoryComponent : MonoBehaviour
    {
        Dictionary<string, List<KeyValuePair<Type, ValueType>>> memoryIdsByTag = new Dictionary<string, List<KeyValuePair<Type, ValueType>>>();
        Dictionary<KeyValuePair<Type, ValueType>, IMemory> memoryById = new Dictionary<KeyValuePair<Type, ValueType>, IMemory>();
        Dictionary<KeyValuePair<Type, ValueType>, HashSet<string>> tagsByMemoryId = new Dictionary<KeyValuePair<Type, ValueType>, HashSet<string>>();

        public T Recall<T>(KeyValuePair<Type, ValueType> id, IMemory def = null) where T : IMemory
        {
            return (T)Recall(id, def);
        }

        public IMemory Recall(KeyValuePair<Type, ValueType> id, IMemory def = null)
        {
            if (!memoryById.ContainsKey(id)) return def;
            return memoryById[id];
        }

        public List<T> Recall<T>(string tag) where T : IMemory
        {
            return memoryIdsByTag.ContainsKey(tag) ? memoryIdsByTag[tag].Select(x => Recall(x)).Cast<T>().ToList() : new List<T>();
        }

        public List<IMemory> Recall(string tag)
        {
            return memoryIdsByTag.ContainsKey(tag) ? memoryIdsByTag[tag].Select(x => Recall(x)).ToList() : new List<IMemory>();
        }

        public List<T> RecallNearby<T>(string tag, Vector3 position, float distance) where T : IMemory, IMemoryLocation
        {
            if (!memoryIdsByTag.ContainsKey(tag)) return new List<T>();
            // TODO: IMplement SpacialIndex/RTREE this is very inefficient!
            return Recall(tag).Cast<IMemoryLocation>().Where(x => Vector3.Distance(x.position, position) <= distance).OrderBy(x => Vector3.Distance(x.position, position)).Cast<T>().ToList();
        }

        public void Remember(IMemory memory, HashSet<string> tags, params string[] additionaltags)
        {
            var fulltags = tags == null ? new HashSet<string>() : new HashSet<string>(tags);
            fulltags.UnionWith(additionaltags);

            if (memoryById.ContainsKey(memory.id)) // manage old tags only if we had previously remembered this
            {
                foreach (string tag in tagsByMemoryId[memory.id]) // remove any tags we are dropping
                    memoryIdsByTag[tag].Remove(memory.id);
            }
            foreach (string tag in fulltags) // ensure all tags are tracked
            {
                if (!memoryIdsByTag.ContainsKey(tag))
                    memoryIdsByTag[tag] = new List<KeyValuePair<Type, ValueType>>() { memory.id };
                else
                    memoryIdsByTag[tag].Insert(0, memory.id); // recent memories first
            }
            tagsByMemoryId[memory.id] = fulltags; // update tags set
            memoryById[memory.id] = memory; // update memoryid
        }

        public void Forget(IMemory memory)
        {
            if (!memoryById.ContainsKey(memory.id)) return;
            memoryById.Remove(memory.id);
            foreach (string tag in tagsByMemoryId[memory.id])
                memoryIdsByTag[tag].Remove(memory.id); // intentionally we're not cleaning empty tags for performance since tags are few and probably will re-appear
            tagsByMemoryId.Remove(memory.id);
        }
    }
}