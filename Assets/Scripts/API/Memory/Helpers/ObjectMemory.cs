using System;
using System.Collections.Generic;
using API.Memory.Contract;
using API.Memory.Internal;
using UnityEngine;

namespace API.Memory.Helpers
{
    public class ObjectMemory : IMemory, IMemoryLocation
    {
        public MemoryID id { get; set; }
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
        public Vector3 scale { get; set; }

        public ObjectMemory(GameObject gameobject)
        {
            id = new MemoryID(this.GetType(), gameobject.GetInstanceID());
            position = gameobject.transform.position;
            rotation = gameobject.transform.rotation;
            scale = gameobject.transform.localScale;
        }
    }
}