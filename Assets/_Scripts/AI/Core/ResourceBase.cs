using System.Collections.Generic;
using UnityEngine;

public abstract class ResourceBase : MonoBehaviour
{
	public float Quantity = 0f;

	public void Start()
	{
		UpdateSize();
	}

	public abstract IResourceManager Manager { get; }

	public virtual void Inc(float amount)
	{
		Quantity += amount;
		UpdateSize();
	}

	public float Take(float max)
	{
		var result = Mathf.Min(Quantity, max);
		Quantity = Mathf.Max(0f, Quantity - max);
		if (Quantity == 0f)
			Manager.Remove(this);
		UpdateSize();
		return result;
	}

	public void UpdateSize()
	{
		this.transform.localScale = new Vector3(this.transform.localScale.x, Quantity, this.transform.localScale.z);
		this.transform.position = new Vector3(this.transform.position.x, this.transform.localScale.y / 2, this.transform.position.z);
	}

}