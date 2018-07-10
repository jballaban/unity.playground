using System.Linq;
using UnityEngine;

public abstract class DepositAction<TDestination, TResource> : ProximityActionBase where TResource : ResourceBase where TDestination : ResourceBase
{
	protected override void Start()
	{
		base.Start();
		this.PreConditions["has" + typeof(TResource).Name] = true;
		this.PreConditions["know" + typeof(TDestination).Name] = true;
		this.Effects["collect" + typeof(TResource).Name] = true;
	}

	public override void DetermineTarget(AgentBase agent)
	{
		var closest = agent.Navigation.GetClosest(agent.Memory.GetAll<ResourceBaseRecollection>(typeof(TDestination)).Select(m => new System.Collections.Generic.KeyValuePair<Vector3, ResourceBaseRecollection>(m.Position, m)).ToList());
		Destination = closest.Value.Key;
		Target = closest.Value.Value;
	}

	public override bool Perform(AgentBase agent)
	{
		if (Target as ResourceBaseRecollection == null) return Failure(agent);
		var resource = (Target as ResourceBaseRecollection).GetOriginal();
		if (resource == null)
		{
			agent.Memory.Forget(Target);
			return Failure(agent);
		}
		resource.Inc(agent.Backpack.Take<TResource>(1f));
		return Failure(agent);
	}

}