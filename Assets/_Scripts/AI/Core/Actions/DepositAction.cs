using UnityEngine;

public class DepositAction<TDestination, TResource> : ActionBase where TResource : ResourceBase
{
    void Start()
    {
        this.PreConditions["has" + typeof(TResource).Name] = true;
        this.PreConditions["know" + typeof(TDestination).Name] = true;
        this.Effects["collect" + typeof(TResource).Name] = true;
        this.RequiresInRange = true;
    }

    public override bool Perform(AgentBase agent)
    {
        agent.Backpack.Inc<TResource>(1f);
        return true;
    }
}