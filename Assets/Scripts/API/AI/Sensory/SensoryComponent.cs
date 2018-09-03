
using API.AI.Sensory.Contract;
using API.AI.Sensory.Internal;
using UnityEngine;

namespace API.AI.Sensory
{
    public class SensoryComponent : MonoBehaviour
    {
        public VisionSensorComponentBase visionSensor;

        void Awake()
        {
            visionSensor = GetComponent<VisionSensorComponentBase>();
            if (visionSensor != null)
            {
                var vision = new GameObject("VisionSensor");
                vision.transform.parent = transform;
                var collider = vision.AddComponent<SphereCollider>();
                collider.radius = visionSensor.range;
                collider.isTrigger = true;
                var handler = vision.AddComponent<TriggerHandler>();
                handler.caller = visionSensor;
            }
        }
    }
}