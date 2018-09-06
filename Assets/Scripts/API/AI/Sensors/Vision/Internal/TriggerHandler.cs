using API.AI.Sensory.Contract;
using UnityEngine;

namespace API.AI.Sensory.Internal
{
	public class TriggerHandler : MonoBehaviour
	{
		public IVisionSensor caller;

		void OnTriggerEnter(Collider other)
		{
			if (caller.IsVisible(other.gameObject))
				caller.Perceive(other.gameObject);
		}

		void OnTriggerExit(Collider other)
		{
			caller.Lose(other.gameObject);
		}
	}
}