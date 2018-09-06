using API.AI.Sensory.Contract;
using UnityEngine;

namespace API.AI.Sensory.Contract
{
	public interface IVisionSensor
	{
		float range { get; }

		void Lose(GameObject other);

		void Perceive(GameObject other);

		bool IsVisible(GameObject other);
	}
}