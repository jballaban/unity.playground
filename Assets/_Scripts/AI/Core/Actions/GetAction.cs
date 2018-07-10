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

    public override void DetermineTarget(AgentBase agent)
    {
        Target = agent.Navigation.GetClosest(agent.Memory.GetAll<ResourceBaseRecollection>(typeof(TResource)).Select(m => new System.Collections.Generic.KeyValuePair<Vector3, ResourceBaseRecollection>(m.Position, m)).ToList());
    }
}