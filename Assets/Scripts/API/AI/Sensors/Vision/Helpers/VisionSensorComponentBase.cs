using System.Collections.Generic;
using API.AI.Sensory.Contract;
using UnityEngine;

namespace API.AI.Sensory.Helpers
{
	public abstract class VisionSensorComponentBase : MonoBehaviour, IVisionSensor
	{
		public float arc = 45f;
		public float _range = 10f;

		public float range
		{
			get { return _range; }
			set { _range = value; }
		}

		void Update()
		{
			Debug.DrawLine(transform.position, transform.position + Vector3.forward * range, Color.red);
		}

		public abstract void Lose(GameObject other);

		public abstract void Perceive(GameObject other);

		public virtual bool IsVisible(GameObject target)
		{
			Debug.Log("IsVislble:" + this.transform.position + "," + target.transform.position);
			if (Vector3.Distance(transform.position, target.transform.position) < range)
			{
				// enemy is within distance
				if (Vector3.Dot(transform.forward, target.transform.position) > 0 && Vector3.Angle(transform.forward, target.transform.position) < arc)
				{
					Debug.Log("arc");
					// enemy is ahead of me and in my field of view
					RaycastHit hitInfo;
					if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hitInfo))
					{
						Debug.Log("got it!");
						// we hit SOMETHING, not necessarily a player
						//if (hitInfo.collider.name == "Person")
						return true;
					}
				}
			}
			return false;
		}
	}
}