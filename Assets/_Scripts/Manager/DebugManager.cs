using UnityEngine;

public class DebugManager : ResourceManager<DebugResource>
{
    public ResourceBase Create(Vector3 position, float qty, IResourceManager manager)
    {
        var resource = manager.Create(position, qty);
        Destroy(resource.GetComponent(resource.GetType()));
        resource.gameObject.AddComponent<DebugResource>();
        return resource;
    }

    public override ResourceBase Create(Vector3 position, float qty)
    {
        // do nothing
        return null;
    }

}