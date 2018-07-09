using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ResourceManager<TResource> : MonoBehaviour where TResource : UnityEngine.Object
{
	List<TResource> Resources = new List<TResource>();
	public void Start()
	{
		Resources = FindObjectsOfType<TResource>().ToList();
	}

	public List<TResource> GetAll()
	{
		return Resources;
	}
}