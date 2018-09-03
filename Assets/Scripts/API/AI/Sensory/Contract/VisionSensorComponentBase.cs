using API.AI.Sensory.Contract;
using UnityEngine;

namespace API.AI.Sensory.Contract
{
    public abstract class VisionSensorComponentBase : MonoBehaviour
    {
        public float range;

        public abstract void Lose(GameObject other);

        public abstract void Perceive(GameObject other);
    }
}