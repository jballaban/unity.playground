using System.Linq;
using UnityEngine;

public class GetAction<TResource> : ProximityActionBase where TResource : ResourceBase
{
	void Start()
	{
		this.PreConditions["has" + typeof(TResource).Name] = false;
		this.PreConditions["know" + typeof(TResource).Name] = true;
		this.Effects["has" + typeof(TResource).Name] = true;
	}

	public override void DetermineTarget(AgentBase agent)
	{
		Target = agent.Navigation.GetClosest(agent.Memory.GetAll<TResource>().Select(s => s.gameObject.transform).ToList());
	}
}