using UnityEngine;

public class ObjectMemory : IMemory
{
	GameObject gameObject;
	Transform transform { get { return gameObject.transform; } }

	public ObjectMemory(GameObject gameobject)
	{
		gameObject = gameobject;
	}

	public int GetInstanceID()
	{
		return gameObject.GetInstanceID();
	}

}