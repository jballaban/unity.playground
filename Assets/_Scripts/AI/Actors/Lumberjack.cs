using UnityEngine;

public class Lumberjack : AgentBase
{
    protected override void Start()
    {
        base.Start();
        foreach (var resource in FindObjectsOfType<TreeResource>())
            Memory.Remember<TreeResource>(resource.gameObject);
        foreach (var resource in FindObjectsOfType<DepotResource>())
            Memory.Remember<DepotResource>(resource.gameObject);
        GoalState.Add("has" + typeof(LogResource).Name, true);
    }
}