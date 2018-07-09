using System.Linq;
using UnityEngine;

public class DepositAction<TDestination, TResource> : ProximityActionBase where TResource : ResourceBase where TDestination : ResourceBase
{
	void Start()
	{
		this.PreConditions["has" + typeof(TResource).Name] = true;
		this.PreConditions["know" + typeof(TDestination).Name] = true;
		this.Effects["collect" + typeof(TResource).Name] = true;
	}

	public override void DetermineTarget(AgentBase agent)
	{
		Target = agent.Navigation.GetClosest(agent.Memory.GetAll<TDestination>().Select(s => s.gameObject.transform).ToList());
	}

	public override bool Perform(AgentBase agent)
	{
		agent.Backpack.Inc<TResource>(1f);
		return true;
	}

}