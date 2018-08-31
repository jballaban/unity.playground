using System;
using System.Collections.Generic;
using API.Memory.Contract;
using UnityEngine;

namespace API.Memory.Helpers
{
	public class ObjectMemory : IMemory
	{
		public IMemoryID id { get; set; }

		internal ObjectMemory(object o) : this((GameObject)o) { }

		public ObjectMemory(GameObject gameobject)
		{
			id = new MemoryID<ObjectMemory>(gameobject.GetInstanceID());
		}
	}
}