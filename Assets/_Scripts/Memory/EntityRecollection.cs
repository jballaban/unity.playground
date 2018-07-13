using UnityEngine;

public class EntityRecollection : IRecollection
{
    public EntityRecollection(object obj)
    {
        Refresh(obj);
    }

    public virtual void Refresh(object obj)
    {
        position = (obj as Transform).position;
        scale = (obj as Transform).localScale;
        rotation = (obj as Transform).rotation;
    }

    public Vector3 position;
    public Vector3 scale;
    public Quaternion rotation;
}