using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SensorySystem))]
public class ResourceMemoryComponent : MemoryComponentBase, DebuggerIListener
{
	State _worldState;
	Dictionary<Type, List<int>> _resourceIds = new Dictionary<Type, List<int>>();

	void Start()
	{
		_worldState = GetComponent<AgentBase>().WorldState;
		GetComponent<SensorySystem>().GetEvent<SensorySystem.ObserveEnterEvent>().AddListener(OnObserveEnter);
	}

	public void FocusDebug()
	{
		Debugger.Instance.Log<ResourceMemoryComponent>("Debug.Focus", gameObject);
		foreach (var type in _resourceIds.Keys)
		{
			foreach (var id in _resourceIds[type])
			{
				var obj = base.Get<ResourceRecollection>(id);
				DebugManager.Instance.CreateFrom(obj.manager.GetTemplate(), obj.position, obj.quantity);
			}
		}
	}

	public void BlurDebug()
	{
		Debugger.Instance.Log<ResourceMemoryComponent>("Debug.Blur", gameObject);
		DebugManager.Instance.Clear();
	}

	public override void Remember<T>(int id, object data)
	{
		var resource = data as ResourceBase;
		if (resource == null)
			throw new Exception(String.Format("Resource Memory is only for saving resources, not other stuff like {0}", data.GetType().Name));
		var type = resource.GetType();
		if (!_resourceIds.ContainsKey(type))
			_resourceIds.Add(type, new List<int>());
		if (!_resourceIds[type].Contains(id))
			_resourceIds[type].Add(id);
		base.Remember<T>(id, data);
		_worldState["know" + type.Name] = true;
	}

	public override void Forget(int id)
	{
		var item = Recall<ResourceRecollection>(id);
		if (!_resourceIds.ContainsKey(item.type))
			throw new Exception(String.Format("Resources list is out of date with memory component, seems wrong.  Trying to forget {0}", item.type));
		_resourceIds[item.type].Remove(id);
		base.Forget(id);
		_worldState["know" + item.type.Name] = _resourceIds[item.type].Count > 0;
	}

	public List<ResourceRecollection> GetAll(Type type)
	{
		if (!_resourceIds.ContainsKey(type))
			return new List<ResourceRecollection>();
		return _resourceIds[type].Select(r => Recall<ResourceRecollection>(r)).ToList();
	}

	void OnObserveEnter(GameObject obj)
	{
		if (obj.CompareTag("interactive"))
		{
			var resource = obj.GetComponent<ResourceBase>();
			if (resource == null) return;
			var id = resource.GetInstanceID();
			Remember<ResourceRecollection>(id, resource);
		}
	}
}