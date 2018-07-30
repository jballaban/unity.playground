using UnityEngine;

public class Lumberjack : AgentBase
{
    void Start()
    {
        GoalState.Add("collect" + typeof(LogResource).Name, true);
    }
}