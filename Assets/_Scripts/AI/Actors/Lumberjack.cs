using UnityEngine;

public class Lumberjack : AgentBase
{
	protected override void Start()
	{
		base.Start();
		GoalState.Add("collect" + typeof(LogResource).Name, true);
	}
}