using System.Linq;
using UnityEngine;

public abstract class ExtractAction<TSource, TOutput> : ProximityActionBase where TSource : ResourceBase where TOutput : ResourceBase
{
    ResourceMemoryComponent _resourceMemory;
    protected override void Start()
    {
        base.Start();
        this.preConditions["know" + typeof(TSource).Name] = true;
        this.preConditions["has" + typeof(TOutput).Name] = false;
        this.effects["know" + typeof(TOutput).Name] = true;
        _resourceMemory = GetComponent<ResourceMemoryComponent>();
    }

    public abstract IResourceManager OutputManager { get; }

    public override void DetermineTarget(AgentBase agent)
    {
        var closest2 = agent.Navigation.GetClosest(_resourceMemory.GetAll(typeof(TSource)).Select(m => new System.Collections.Generic.KeyValuePair<Vector3, ResourceRecollection>(m.position, m)).ToList());
        if (closest2 == null)
            return;
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
        var qty = original.Take(4f);
        if (original.Quantity == 0f)
        { // destroyed
            _resourceMemory.Forget(Target.id);
        }
        if (qty > 0)
            OutputManager.Create(Target.position + new Vector3(-2, 0, 0), Mathf.Min(1f, qty));
        if (qty > 1)
            OutputManager.Create(Target.position + new Vector3(2, 0, 0), Mathf.Min(1f, qty - 1f));
        if (qty > 2)
            OutputManager.Create(Target.position + new Vector3(0, 0, 2), Mathf.Min(1f, qty - 2f));
        if (qty > 3)
            OutputManager.Create(Target.position + new Vector3(0, 0, -2), Mathf.Min(1f, qty - 3f));
        return Success(agent);
    }

}