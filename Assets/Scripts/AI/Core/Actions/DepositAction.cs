using System.Linq;
using UnityEngine;

public abstract class DepositAction<TDestination, TResource> : ProximityActionBase where TResource : ResourceBase where TDestination : ResourceBase
{
    ResourceMemoryComponent _resourceMemory;
    protected override void Start()
    {
        base.Start();
        this.PreConditions["has" + typeof(TResource).Name] = true;
        this.PreConditions["know" + typeof(TDestination).Name] = true;
        this.Effects["collect" + typeof(TResource).Name] = true;
        _resourceMemory = GetComponent<ResourceMemoryComponent>();
    }

    public override void DetermineTarget(AgentBase agent)
    {
        var closest2 = agent.Navigation.GetClosest(_resourceMemory.GetAll(typeof(TDestination)).Select(m => new System.Collections.Generic.KeyValuePair<Vector3, ResourceRecollection>(m.position, m)).ToList());
        Destination = closest2.Value.Key;
        Target = closest2.Value.Value;
    }

    public override bool Perform(AgentBase agent)
    {
        var resource = Target as ResourceRecollection;
        if (resource == null) return Failure(agent);
        var original = resource.manager.FindByID(resource.id);
        if (original == null)
        {
            _resourceMemory.Forget(resource.id);
            return Failure(agent);
        }
        original.Inc(agent.Backpack.Take<TResource>(1f));
        return Failure(agent);
    }

}