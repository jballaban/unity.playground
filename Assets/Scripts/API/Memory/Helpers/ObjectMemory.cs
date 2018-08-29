using System;
using System.Collections.Generic;
using API.Memory.Contract;
using UnityEngine;

namespace API.Memory.Helpers
{
	public class ObjectMemory : IMemory, IMemoryLocation
	{
		public KeyValuePair<Type, ValueType> id { get; set; }
		public Vector3 position { get; set; }
		public Quaternion rotation { get; set; }
		public Vector3 scale { get; set; }

		public ObjectMemory(GameObject gameobject)
		{
			id = new KeyValuePair<Type, ValueType>(this.GetType(), gameobject.GetInstanceID());
			position = gameobject.transform.position;
			rotation = gameobject.transform.rotation;
			scale = gameobject.transform.localScale;
		}
	}
}