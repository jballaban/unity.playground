using System;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
	Dictionary<string, HashSet<IMemory>> memoriesByTag = new Dictionary<string, HashSet<IMemory>>();
	Dictionary<int, IMemory> memoriesById = new Dictionary<int, IMemory>();
	Dictionary<IMemory, HashSet<string>> tagsByMemory = new Dictionary<IMemory, HashSet<string>>();

	public IMemory Recall(int id)
	{
		if (!memoriesById.ContainsKey(id))
			throw new Exception($"Could not find memory id {id}");
		return memoriesById[id];
	}

	public HashSet<IMemory> Recall(string tag)
	{
		return memoriesByTag.ContainsKey(tag) ? memoriesByTag[tag] : new HashSet<IMemory>();
	}

	public HashSet<string> GetTags(IMemory memory)
	{
		if (!tagsByMemory.ContainsKey(memory))
			throw new Exception("Memory has no tags!");
		return tagsByMemory[memory];
	}

	public void Remember(HashSet<string> tags, IMemory memory)
	{
		// todo: if remembering the same object again we have to remove diff
		if (!memoriesByTag.ContainsKey(tag))
			memoriesByTag[tag] = new HashSet<IMemory>() { memory };
		else
			memoriesByTag[tag].Add(memory);
		if (!tagsByMemory.ContainsKey(memory))
			tagsByMemory[memory] = new HashSet<string>() { tag };
		else
			tagsByMemory[memory].Add(tag);
		memoriesById[memory.GetInstanceID()] = memory;
	}

	public void AddTag(string tag, IMemory memory)
	{
		if (!memoriesByTag.ContainsKey(tag))
			memoriesByTag[tag] = new HashSet<IMemory>() { memory };
		else
			memoriesByTag[tag].Add(memory);
		if (!tagsByMemory.ContainsKey(memory))
			throw new Exception("Cannot add tag to a memory that isn't currently remembered");
		tagsByMemory[memory].Add(tag);
		memoriesById[memory.GetInstanceID()] = memory;
	}

	public void RemoveTag(string tag, IMemory memory)
	{
		if (!tagsByMemory.ContainsKey(memory))
			throw new Exception("How are we forgetting something we never knew!");
		if (!tagsByMemory[memory].Contains(tag))
			throw new Exception($"Trying to remove a tag that doesn't exist on a memory! {tag}");
		tagsByMemory[memory].Remove(tag);
		if (tagsByMemory[memory].Count == 0)
			Forget(memory);
	}

	public void Forget(IMemory memory)
	{
		if (!tagsByMemory.ContainsKey(memory))
			throw new Exception("How are we forgetting something we never knew!");
		foreach (string tag in tagsByMemory[memory])
		{
			if (!memoriesByTag.ContainsKey(tag))
				throw new Exception($"Somehow a memory exists in a tag we don't know about {tag}");
			memoriesByTag[tag].Remove(memory);
			if (memoriesByTag[tag].Count == 0)
				memoriesByTag.Remove(tag);
		}
		tagsByMemory.Remove(memory);
		if (!memoriesById.ContainsKey(memory.GetInstanceID()))
			throw new Exception($"Missing id cache on memory {memory.GetInstanceID()}");
		memoriesById.Remove(memory.GetInstanceID());
	}
}