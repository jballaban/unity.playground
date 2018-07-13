using System;
using UnityEngine;

public class ResourceRecollection : EntityRecollection
{

    public ResourceRecollection(object obj) : base(obj) { }

    public override void Refresh(object obj)
    {
        quantity = (obj as ResourceBase).Quantity;
        resourceType = (obj as ResourceBase).GetType();
    }

    public float quantity;

    public Type resourceType;
}