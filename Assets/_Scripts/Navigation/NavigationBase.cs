using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavigationBase : MonoBehaviour
{
	public NavMeshAgent _nav;

	public void Start()
	{
		_nav = this.GetComponent<NavMeshAgent>();
	}

	public Transform GetClosest(List<Transform> locations)
	{
		if (locations.Count == 0)
			return null;

		var closest = locations[0];
		var distance = Vector3.Distance(this.transform.position, closest.position);
		foreach (var location in locations)
		{
			var d = Vector3.Distance(this.transform.position, closest.position);
			if (d < distance)
			{
				distance = d;
				closest = location;
			}
		}
		return closest;
	}

	public bool Valid()
	{
		return _nav.hasPath;
	}

	public float RemainingDistance()
	{
		return _nav.remainingDistance;
	}

	public bool GoTo(Vector3 destination)
	{
		return _nav.SetDestination(destination);
	}

	public void Stop()
	{
		_nav.isStopped = true;
	}
}