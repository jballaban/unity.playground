using System.Linq;
using UnityEngine;

public class SearchAction<TResource> : ProximityActionBase where TResource : ResourceBase
{

    protected override void Start()
    {
        base.Start();
        this.PreConditions["has" + typeof(TResource).Name] = false;
        this.PreConditions["know" + typeof(TResource).Name] = false;
        this.Effects["know" + typeof(TResource).Name] = true;
        this.Cost = 10f;
    }

    public override bool Perform(AgentBase agent)
    {
        if (!agent.WorldState.ContainsKey("know" + typeof(TResource).Name))
            return Failure(agent);
        return Success(agent);
    }

    public override void DetermineTarget(AgentBase agent)
    {
        var moveto = Random.insideUnitCircle * 10;
        Destination = agent.gameObject.transform.position + new Vector3(moveto.x, 0, moveto.y);
    }
}