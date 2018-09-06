
using API.AI.Sensory.Contract;
using API.AI.Sensory.Internal;
using UnityEngine;

namespace API.AI.Sensory
{
	public class SensoryComponent : MonoBehaviour
	{
		public IVisionSensor visionSensor;
		public GameObject visionSensorPrefab;

		void Awake()
		{
			InitPrefabs();
			visionSensor = GetComponent<IVisionSensor>();
			if (visionSensor != null)
			{
				var child = GameObject.Instantiate(visionSensorPrefab, transform);
				child.GetComponent<SphereCollider>().radius = visionSensor.range;
				child.GetComponent<TriggerHandler>().caller = visionSensor;
				child.SetActive(true);
			}
		}

		void InitPrefabs()
		{
			if (visionSensorPrefab == null)
			{
				visionSensorPrefab = new GameObject("VisionSensor");
				visionSensorPrefab.SetActive(false);
				visionSensorPrefab.AddComponent<SphereCollider>().isTrigger = true;
				visionSensorPrefab.AddComponent<TriggerHandler>();
			}
		}
	}
}