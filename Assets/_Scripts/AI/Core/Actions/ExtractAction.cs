using UnityEngine;

public abstract class ExtractAction<TSource, TOutput> : ActionBase where TSource : ResourceBase where TOutput : ResourceBase
{
    void Start()
    {
        this.PreConditions["know" + typeof(TSource).Name] = true;
        this.PreConditions["has" + typeof(TOutput).Name] = false;
        this.Effects["has" + typeof(TOutput).Name] = true;
        this.RequiresInRange = true;
    }

    public override abstract bool Perform(AgentBase agent);
}