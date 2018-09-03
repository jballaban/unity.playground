using System;
using System.Collections.Generic;
using API.AI.Memory.Contract;
using UnityEngine;

namespace API.AI.Memory.Helpers
{
    public class PlaceMemory : ILocationMemory
    {
        public IMemoryID id { get; set; }
        public Vector3 position { get; set; }

        internal PlaceMemory(object position) : this((Vector3)position) { }

        public PlaceMemory(Vector3 position)
        {
            this.position = position;
            this.id = new MemoryID<PlaceMemory>(this.position);
        }

        public bool Intersects(Vector3 center, float radius)
        {
            return Vector3.Distance(this.position, center) <= radius;
        }
    }
}