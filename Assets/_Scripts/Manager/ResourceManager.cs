using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IResourceManager
{
    void Remove(ResourceBase resource);
    ResourceBase FindByID(int id);
    ResourceBase Create(Vector3 position, float qty);
}

public abstract class ResourceManager<TResource> : MonoBehaviour, IResourceManager where TResource : ResourceBase
{
    public void Awake()
    {
        ManagerBase.Instance.Register(this);
    }

    public Transform Template;

    public Transform Root;

    List<ResourceBase> _resources = null;

    public List<ResourceBase> Resources
    {
        get
        {
            if (_resources == null)
                _resources = FindObjectsOfType<TResource>().Cast<ResourceBase>().ToList();
            return _resources;
        }
    }

    public virtual void Remove(ResourceBase resource)
    {
        Resources.Remove(resource);
        Destroy(resource.gameObject);
    }

    public virtual ResourceBase Create(Vector3 position, float qty)
    {
        var obj = Object.Instantiate(Template, Root);
        obj.position = position;
        var resource = obj.GetComponent<ResourceBase>();
        Resources.Add(resource);
        resource.Inc(qty);
        return resource;

    }

    public ResourceBase FindByID(int id)
    {
        return Resources.FirstOrDefault(r => r.GetInstanceID() == id);
    }
}