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

    public T GetClosest<T>(List<KeyValuePair<Vector3, T>> kvp) where T : class
    {
        if (kvp.Count == 0)
            return null;

        var closest = kvp[0].Value;
        var distance = Vector3.Distance(this.transform.position, kvp[0].Key);
        foreach (var pair in kvp)
        {
            var d = Vector3.Distance(this.transform.position, pair.Key);
            if (d < distance)
            {
                distance = d;
                closest = pair.Value;
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