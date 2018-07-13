using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class VisionComponent : ComponentBase
{
	SensorySystem _sensorySystem;
	void Awake()
	{
		_sensorySystem = GetComponentInParent<SensorySystem>();
		if (_sensorySystem == null)
			throw new Exception("SensorySystem required to be a parent of VisionComponent");
		_sensorySystem.RegisterEvent(new SensorySystem.ObserveEnterEvent());
		_sensorySystem.RegisterEvent(new SensorySystem.ObserveExitEvent());
	}

	void OnTriggerEnter(Collider other)
	{
		_sensorySystem.GetEvent<SensorySystem.ObserveEnterEvent>().Invoke(other.gameObject);
	}

	void OnTriggerExit(Collider other)
	{
		_sensorySystem.GetEvent<SensorySystem.ObserveExitEvent>().Invoke(other.gameObject);
	}

}