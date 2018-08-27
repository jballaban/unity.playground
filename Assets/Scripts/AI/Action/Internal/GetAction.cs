using System;
using System.Linq;
using UnityEngine;

public class GetAction<TResource> : ProximityActionBase where TResource : ResourceBase
{
    ResourceMemoryComponent _resourceMemory;

    protected override void Start()
    {
        base.Start();
        this.preConditions["has" + typeof(TResource).Name] = false;
        this.preConditions["know" + typeof(TResource).Name] = true;
        this.effects["has" + typeof(TResource).Name] = true;
        _resourceMemory = GetComponent<ResourceMemoryComponent>();
    }

    public override void DetermineTarget(AgentBase agent)
    {
        var closest2 = agent.Navigation.GetClosest(_resourceMemory.GetAll(typeof(TResource)).Select(m => new System.Collections.Generic.KeyValuePair<Vector3, ResourceRecollection>(m.position, m)).ToList());
        Destination = closest2.Value.Key;
        Target = closest2.Value.Value;
    }

    public override bool Perform(AIActionIAgent agent)
    {
        var resource = Target as ResourceRecollection;
        if (resource == null) return Failure(agent);
        var original = resource.manager.FindByID(resource.id);
        if (original == null)
        {
            _resourceMemory.Forget(Target.id);
            return Failure(agent);
        }
        (agent as AgentBase).Backpack.Inc<TResource>(original.Take(1f));
        if (original.Quantity == 0f)
        { // empty
            _resourceMemory.Forget(Target.id);
        }
        return Success(agent);
    }

}