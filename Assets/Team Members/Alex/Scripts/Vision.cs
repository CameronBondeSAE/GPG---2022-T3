using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class Vision : MonoBehaviour
    {
        public int rays = 10;
        public float spacingScale = 1f;
        public List<Transform> resourcesInSight;
        public List<Transform> otherPlayersInSight;
        public GameObject currentTarget;
        public Sensor sensor;
        

        private void FixedUpdate()
        {
            resourcesInSight.Clear();
            for (int i = -rays; i < rays; i++)
            {
                
                // Very simple. Doesn't take any tilting or pitching into account, but is fine for horizontal only AIs
                Vector3 dir = Quaternion.Euler(0, i*spacingScale, 0) * transform.forward;
    
                //Debug.DrawRay(transform.position, dir * 10f, Color.green);

                Physics.Raycast(transform.position, dir, out RaycastHit HitInfo);
                if(HitInfo.collider == null) continue;
                Debug.DrawLine(transform.position, HitInfo.point, Color.green);
                
                if (HitInfo.collider.GetComponent<Resource>() != null)
                {
                    Transform resource = HitInfo.transform;

                    if (!resourcesInSight.Contains(resource))
                    {
                        resourcesInSight.Add(resource);
                    }
                }
            }
        }
    }
}
