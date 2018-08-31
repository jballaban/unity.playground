using System;
using System.Collections.Generic;
using API.Memory.Contract;
using UnityEngine;

public class PlaceMemory : IMemory
{
	public IMemoryID id { get; set; }
	public Vector3 position { get; set; }

	internal PlaceMemory(object position) : this((Vector3)position) { }

	public PlaceMemory(Vector3 position)
	{
		this.position = position;
		this.id = new MemoryID<PlaceMemory>(this.position);
	}
}