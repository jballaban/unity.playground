using System.Linq;
using UnityEngine;

public abstract class ExtractAction<TSource, TOutput> : ProximityActionBase where TSource : ResourceBase where TOutput : ResourceBase
{
	void Start()
	{
		this.PreConditions["know" + typeof(TSource).Name] = true;
		this.PreConditions["has" + typeof(TOutput).Name] = false;
		this.Effects["has" + typeof(TOutput).Name] = true;
	}

	public override void DetermineTarget(AgentBase agent)
	{
		Target = agent.Navigation.GetClosest(agent.Memory.GetAll<TSource>().Select(s => s.gameObject.transform).ToList());
	}

	public override abstract bool Perform(AgentBase agent);
}