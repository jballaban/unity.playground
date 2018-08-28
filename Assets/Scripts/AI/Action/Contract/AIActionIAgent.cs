using UnityEngine;

public interface AIActionIAgent
{
    GameObject gameObject { get; }
    NavigationSystem Navigation { get; }
}