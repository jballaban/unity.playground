using System;
using System.Linq;
using UnityEngine;

public class GetAction<TResource> : ProximityActionBase where TResource : ResourceBase
{

	protected override void Start()
	{
		base.Start();
		this.PreConditions["has" + typeof(TResource).Name] = false;
		this.PreConditions["know" + typeof(TResource).Name] = true;
		this.Effects["has" + typeof(TResource).Name] = true;
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
		agent.Backpack.Inc<TResource>(resource.Take(1f));
		if (resource.Quantity == 0f) // empty
			agent.Memory.Forget(Target);
		return Success(agent);
	}

	public override void DetermineTarget(AgentBase agent)
	{
		var closest = agent.Navigation.GetClosest(agent.Memory.GetAll<ResourceBaseRecollection>(typeof(TResource)).Select(m => new System.Collections.Generic.KeyValuePair<Vector3, ResourceBaseRecollection>(m.Position, m)).ToList());
		if (closest == null)
			throw new Exception("Error: " + this.GetType() + " could not find resource");
		Destination = closest.Value.Key;
		Target = closest.Value.Value;
	}
}