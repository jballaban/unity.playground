using System;
using System.Collections.Generic;
using API.Memory.Contract;
using API.Memory.Internal;
using UnityEngine;

public class PlaceMemory : IMemory, IMemoryLocation
{
    public MemoryID id { get; set; }
    public Vector3 position { get; set; }
    public PlaceMemory(Vector3 position)
    {
        this.position = position;
        this.id = new MemoryID(this.GetType(), this.position);
    }
}