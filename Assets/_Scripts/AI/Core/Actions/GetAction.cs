using UnityEngine;

public class GetAction<TResource> : ActionBase where TResource : ResourceBase
{
    void Start()
    {
        this.PreConditions["has" + typeof(TResource).Name] = false;
        this.PreConditions["know" + typeof(TResource).Name] = true;
        this.Effects["has" + typeof(TResource).Name] = true;
        this.RequiresInRange = true;
    }
}