using API.AI.Sensory.Contract;
using UnityEngine;

namespace API.AI.Sensory.Internal
{
    public class TriggerHandler : MonoBehaviour
    {
        public IVisionSensorComponent caller;

        void OnTriggerEnter(Collider other)
        {
            caller.Perceive(other.gameObject);
        }

        void OnTriggerExit(Collider other)
        {
            caller.Lose(other.gameObject);
        }
    }
}