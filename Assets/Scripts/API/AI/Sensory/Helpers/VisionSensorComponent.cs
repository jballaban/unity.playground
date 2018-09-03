using API.AI.Sensory.Contract;
using UnityEngine;

namespace API.AI.Sensory.Helpers
{
    public class VisionSensorComponent : VisionSensorComponentBase
    {
        public float arc = 45f;
        public override void Lose(GameObject other)
        {
            throw new System.NotImplementedException();
        }

        public override void Perceive(GameObject other)
        {
            throw new System.NotImplementedException();
        }

        bool canSee(GameObject target)
        {

            if (Vector3.Distance(transform.position, target.transform.position) < range)
            {
                // enemy is within distance
                if (Vector3.Dot(transform.forward, target.transform.position) > 0 && Vector3.Angle(transform.forward, target.transform.position) < arc)
                {
                    // enemy is ahead of me and in my field of view
                    RaycastHit hitInfo;
                    if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hitInfo) == true)
                    {
                        // we hit SOMETHING, not necessarily a player
                        if (hitInfo.collider.name == "Person")
                            return true;
                    }
                }
            }
            return false;
        }
    }
}