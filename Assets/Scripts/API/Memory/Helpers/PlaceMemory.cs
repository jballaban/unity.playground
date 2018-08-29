using System;
using System.Collections.Generic;
using API.Memory.Contract;
using UnityEngine;

public class PlaceMemory : IMemory, IMemoryLocation
{
	public KeyValuePair<Type, ValueType> id { get; set; }
	public Vector3 position { get; set; }
	public PlaceMemory(Vector3 position)
	{
		this.position = position;
		this.id = new KeyValuePair<Type, ValueType>(this.GetType(), this.position);
	}
}