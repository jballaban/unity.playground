using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ResourceManager : MonoBehaviour
{
    public Transform Template;
    public Transform Root;
    protected virtual void Awake()
    {
        ManagerBase.Instance.Register(this);
    }

    public List<ResourceBase> Resources = new List<ResourceBase>();

    public void Remove(ResourceBase resource)
    {
        Resources.Remove(resource);
        Destroy(resource.gameObject);
    }

    public Transform Create()
    {
        return Object.Instantiate(Template, Root);
    }

    public ResourceBase FindByID(int id)
    {
        return Resources.FirstOrDefault(r => r.GetInstanceID() == id);
    }
}

public abstract class ResourceManager<TResource> : ResourceManager where TResource : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        Resources = FindObjectsOfType<TResource>().Cast<ResourceBase>().ToList();
    }

    public List<TResource> GetAll()
    {
        return Resources.Cast<TResource>().ToList();
    }
}