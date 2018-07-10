using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceBase : MonoBehaviour
{
    public float Quantity = 0f;
    public abstract ResourceManager Manager { get; }

    public float Take(float max)
    {
        var result = Mathf.Min(Quantity, max);
        Quantity = Mathf.Max(0f, Quantity - max);
        if (Quantity == 0)
            Manager.Remove(this);
        return result;
    }

}