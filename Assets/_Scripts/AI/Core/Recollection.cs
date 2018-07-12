
using System;
using UnityEngine;

public abstract class Recollection
{
    public Type Type;
    public int InstanceID;
    public enum Statuses
    {
        New,
        Updated
    }
    public Statuses Status;
}

public class ResourceBaseRecollection : Recollection
{
    public ResourceBaseRecollection(ResourceBase resource)
    {
        InstanceID = resource.GetInstanceID();
        Manager = resource.Manager;
        Type = resource.GetType();
        Refresh(resource);
        Status = Statuses.New;
    }
    public float Quantity;
    public Vector3 Position;
    public Vector3 Scale;
    public Quaternion Rotation;
    public IResourceManager Manager;
    public ResourceBase GetOriginal()
    {
        return Manager.FindByID(InstanceID);
    }

    public void Refresh(ResourceBase resource)
    {
        Quantity = resource.Quantity;
        Position = resource.transform.position;
        Scale = resource.transform.localScale;
        Rotation = resource.transform.rotation;
        Status = Statuses.Updated;
    }
}