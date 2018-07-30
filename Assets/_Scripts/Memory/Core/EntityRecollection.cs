using UnityEngine;

public class EntityRecollection : IRecollection
{
    public EntityRecollection(int id, object obj)
    {
        Refresh(obj);
        this.id = id;
    }

    public virtual void Refresh(object obj)
    {
        position = (obj as GameObject).transform.position;
        scale = (obj as GameObject).transform.localScale;
        rotation = (obj as GameObject).transform.rotation;
        this.id = id;
    }

    public int id { get; set; }
    public Vector3 position;
    public Vector3 scale;
    public Quaternion rotation;
}