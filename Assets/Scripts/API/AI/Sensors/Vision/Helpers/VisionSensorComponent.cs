using System.Collections.Generic;
using UnityEngine;

namespace API.AI.Sensory.Helpers
{
	public class VisionSensorComponent : VisionSensorComponentBase
	{
		public HashSet<int> known = new HashSet<int>();

		public override void Lose(GameObject other)
		{
			known.Remove(other.GetInstanceID());
		}

		public override void Perceive(GameObject other)
		{
			known.Add(other.GetInstanceID());
		}

		public override bool IsVisible(GameObject target)
		{
			return base.IsVisible(target);
		}
	}
}