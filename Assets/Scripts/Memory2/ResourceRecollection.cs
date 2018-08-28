using System;
using UnityEngine;

public class ResourceRecollection : EntityRecollection
{

    public ResourceRecollection(int id, object obj) : base(id, obj) { }

    public override void Refresh(object obj)
    {
        var resource = obj as ResourceBase;
        base.Refresh(resource.gameObject);
        quantity = resource.Quantity;
        type = resource.GetType();
        manager = resource.Manager;
    }

    public float quantity;

    public Type type;
    public IResourceManager manager;
}