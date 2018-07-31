using System;
using UnityEngine;

public class DebugManager : ResourceManager<DebugResource>
{
    public Material material;

    static DebugManager _instance;
    public static DebugManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<DebugManager>();
            return _instance;
        }
    }

    public DebugResource CreateFrom(Transform template, Vector3 position, float qty)
    {
        var obj = UnityEngine.Object.Instantiate(template, Vector3.zero, template.rotation, Root).gameObject;
        UnityEngine.Object.DestroyImmediate(obj.GetComponent<ResourceBase>());
        UnityEngine.Object.DestroyImmediate(obj.GetComponent<Rigidbody>());
        UnityEngine.Object.DestroyImmediate(obj.GetComponent<Collider>());
        var mesh = obj.GetComponent<Renderer>();
        mesh.materials = new Material[] { material };
        obj.transform.position = position;
        var result = obj.AddComponent<DebugResource>();
        Resources.Add(result);
        result.Inc(qty);
        return result;
    }

    public override ResourceBase Create(Vector3 position, float qty)
    {
        throw new NotImplementedException("Use CreateFrom instead");
    }

    public void Clear()
    {
        foreach (var debug in base.Resources)
        {
            UnityEngine.Object.Destroy(debug.gameObject);
        }
        base.Resources.Clear();
    }
}