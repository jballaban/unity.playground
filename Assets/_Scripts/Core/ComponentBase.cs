using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public abstract class ComponentBase : MonoBehaviour
{
	/* 	List<UnityEventBase> _events = new List<UnityEventBase>();

		protected void RegisterEvent(UnityEventBase e)
		{
			_events.Add(e);
		}

		public T GetEvent<T>() where T : UnityEventBase
		{
			var result = _events.FirstOrDefault(e => e is T);
			if (result == null)
				throw new Exception(String.Format("Attempted to retrieve unknown event {0} on {1}", typeof(T).Name, this.GetType().Name));
			return (T)result;
		} */
}