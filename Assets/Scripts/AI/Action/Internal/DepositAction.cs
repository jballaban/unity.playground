using System.Linq;
using UnityEngine;

public abstract class DepositAction<TDestination, TResource> : AIActionProximityBase where TResource : ResourceBase where TDestination : ResourceBase
{
    ResourceMemoryComponent _resourceMemory;
    protected override void Start()
    {
        base.Start();
        this.preConditions["has" + typeof(TResource).Name] = true;
        this.preConditions["know" + typeof(TDestination).Name] = true;
        this.effects["collect" + typeof(TResource).Name] = true;
        _resourceMemory = GetComponent<ResourceMemoryComponent>();
    }

    public override void DetermineTarget(AIActionIAgent agent)
    {
        var closest2 = agent.Navigation.GetClosest(_resourceMemory.GetAll(typeof(TDestination)).Select(m => new System.Collections.Generic.KeyValuePair<Vector3, ResourceRecollection>(m.position, m)).ToList());
        destination = closest2.Value.Key;
        target = closest2.Value.Value;
    }

    public override bool Perform(AIActionIAgent agent)
    {
        var resource = target as ResourceRecollection;
        if (resource == null) return Failure(agent);
        var original = resource.manager.FindByID(resource.id);
        if (original == null)
        {
            _resourceMemory.Forget(resource.id);
            return Failure(agent);
        }
        original.Inc((agent as AgentBase).Backpack.Take<TResource>(1f));
        return Failure(agent);
    }

}