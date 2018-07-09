using UnityEngine;

public class Lumberjack : AgentBase
{
	ManagerBase _managers;
	protected override void Start()
	{
		base.Start();
		_managers = FindObjectOfType<ManagerBase>();
		foreach (var resource in _managers.Depots.GetAll())
			Memory.Remember(resource);
		foreach (var resource in _managers.Trees.GetAll())
			Memory.Remember(resource);
		GoalState.Add("has" + typeof(LogResource).Name, true);
	}
}