using UnityEngine;

public class Lumberjack : AgentBase
{
	protected override void Start()
	{
		base.Start();
		foreach (var resource in ManagerBase.Instance.GetManager<DepotManager>().Resources)
			Memory.Remember(resource);
		foreach (var resource in ManagerBase.Instance.GetManager<TreeManager>().Resources)
			Memory.Remember(resource);
		GoalState.Add("collect" + typeof(LogResource).Name, true);
	}
}