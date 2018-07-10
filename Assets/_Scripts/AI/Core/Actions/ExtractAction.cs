using System.Linq;
using UnityEngine;

public abstract class ExtractAction<TSource, TOutput> : ProximityActionBase where TSource : ResourceBase where TOutput : ResourceBase
{
	protected override void Start()
	{
		base.Start();
		this.PreConditions["know" + typeof(TSource).Name] = true;
		this.PreConditions["has" + typeof(TOutput).Name] = false;
		this.Effects["know" + typeof(TOutput).Name] = true;
	}

	public abstract IResourceManager OutputManager { get; }

	public override void DetermineTarget(AgentBase agent)
	{
		var closest = agent.Navigation.GetClosest(agent.Memory.GetAll<ResourceBaseRecollection>(typeof(TSource)).Select(m => new System.Collections.Generic.KeyValuePair<Vector3, ResourceBaseRecollection>(m.Position, m)).ToList());
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
		var qty = resource.Take(4f);
		if (resource.Quantity == 0f) // destroyed
			agent.Memory.Forget(Target as Recollection);
		if (qty > 0)
			agent.Memory.Remember(OutputManager.Create((Target as ResourceBaseRecollection).Position + new Vector3(-2, 0, 0), Mathf.Min(1f, qty)));
		if (qty > 1)
			agent.Memory.Remember(OutputManager.Create((Target as ResourceBaseRecollection).Position + new Vector3(2, 0, 0), Mathf.Min(1f, qty - 1f)));
		if (qty > 2)
			agent.Memory.Remember(OutputManager.Create((Target as ResourceBaseRecollection).Position + new Vector3(0, 0, 2), Mathf.Min(1f, qty - 2f)));
		if (qty > 3)
			agent.Memory.Remember(OutputManager.Create((Target as ResourceBaseRecollection).Position + new Vector3(0, 0, -2), Mathf.Min(1f, qty - 3f)));
		return Success(agent);
	}

}