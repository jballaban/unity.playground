using System;
using UnityEngine;

public class MemoryComponent : ComponentBase
{
	void Start()
	{
		GetComponent<SensorySystem>().GetEvent<SensorySystem.ObserveEnterEvent>().AddListener(OnObserveEnter);
	}

	void OnObserveEnter(GameObject obj)
	{
		if (obj.CompareTag("interactive"))
			Debug.Log("Found" + obj.name);
	}

}