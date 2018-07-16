using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavigationSystem : SubSystem
{
    public NavMeshAgent _nav;

    public void Start()
    {
        _nav = this.GetComponent<NavMeshAgent>();
    }

    public KeyValuePair<Vector3, T>? GetClosest<T>(List<KeyValuePair<Vector3, T>> kvp) where T : class
    {
        if (kvp.Count == 0)
            return null;

        var closest = kvp[0];
        var distance = Vector3.Distance(this.transform.position, kvp[0].Key);
        foreach (var pair in kvp)
        {
            var d = Vector3.Distance(this.transform.position, pair.Key);
            if (d < distance)
            {
                distance = d;
                closest = pair;
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
        //Debug.Log("Navigation: start");
        _nav.isStopped = false;
        return _nav.SetDestination(destination);
    }

    public void Stop()
    {
        //Debug.Log("Navigation: stop");
        _nav.isStopped = true;
    }
}