using System.Linq;
using UnityEngine;

public abstract class ExtractAction<TSource, TOutput> : ProximityActionBase where TSource : ResourceBase where TOutput : ResourceBase
{
    protected override void Start()
    {
        base.Start();
        this.PreConditions["know" + typeof(TSource).Name] = true;
        this.PreConditions["has" + typeof(TOutput).Name] = false;
        this.Effects["has" + typeof(TOutput).Name] = true;
    }

    public abstract ResourceManager OutputManager { get; }

    public override void DetermineTarget(AgentBase agent)
    {
        Target = agent.Navigation.GetClosest(agent.Memory.GetAll<ResourceBaseRecollection>(typeof(TSource)).Select(m => new System.Collections.Generic.KeyValuePair<Vector3, ResourceBaseRecollection>(m.Position, m)).ToList());
    }

    public override bool Perform(AgentBase agent)
    {
        var resource = Target.GetOriginal();
        if (resource == null) return false;

        var qty = resource.Take(4f);
        if (qty > 0)
        {
            var obj = OutputManager.Create();
            obj.position = Target.Position + Vector3.left;
        }
        if (qty > 1)
        {
            var obj = OutputManager.Create();
            obj.position = Target.Position + Vector3.right;
        }
        if (qty > 2)
        {
            var obj = OutputManager.Create();
            obj.position = Target.Position + Vector3.up;
        }
        if (qty > 3)
        {
            var obj = OutputManager.Create();
            obj.position = Target.Position + Vector3.down;
        }
        //agent.Backpack.Inc<TOutput>(Target.Take(1f));
        return true;
    }
}