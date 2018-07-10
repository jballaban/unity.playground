using UnityEngine;

public class Lumberjack : AgentBase
{
    protected override void Start()
    {
        base.Start();
        foreach (var resource in ManagerBase.Instance.GetManager<DepotManager>().GetAll())
            Memory.Remember(resource);
        foreach (var resource in ManagerBase.Instance.GetManager<TreeManager>().GetAll())
            Memory.Remember(resource);
        GoalState.Add("has" + typeof(LogResource).Name, true);
    }
}