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
        public List<Transform> enemyInSight;
        public List<Transform> dropOffPointsFound;
        public GameObject currentTarget;
        public Sensor sensor;
        public FollowPath followPath;
        public AStar astar;
        public Controller controller;
        
        

        private void FixedUpdate()
        {
            enemyInSight.Clear();
            resourcesInSight.Clear();
            for (int i = -rays; i < rays; i++)
            {
                
                // Very simple. Doesn't take any tilting or pitching into account, but is fine for horizontal only AIs
                Vector3 dir = Quaternion.Euler(0, i*spacingScale, 0) * transform.forward;
    
                //Debug.DrawRay(transform.position, dir * 10f, Color.green);

                Physics.Raycast(transform.position, dir, out RaycastHit HitInfo);
                if(HitInfo.collider == null) continue;
                
                
                if (HitInfo.collider.GetComponent<Enemy>() != null)
                {
                    Debug.DrawLine(transform.position, HitInfo.point, Color.red);
                    Transform enemy = HitInfo.transform;

                    if (!enemyInSight.Contains(enemy))
                    {
                        enemyInSight.Add(enemy);
                        astar.FindPathStartCoroutine(Vector3Int.FloorToInt(controller.rb.transform.position) , Vector3Int.FloorToInt(enemyInSight[0].transform.position));
                    }
                }
                
                else
                {
                    Debug.DrawLine(transform.position, HitInfo.point, Color.green);

                    if (HitInfo.collider.GetComponent<Resource>() != null)
                    {

                        Transform resource = HitInfo.transform;

                        if (!resourcesInSight.Contains(resource))
                        {
                            resourcesInSight.Add(resource);
                            astar.FindPathStartCoroutine(Vector3Int.FloorToInt(controller.rb.transform.position) , Vector3Int.FloorToInt(resourcesInSight[0].transform.position));
                        }
                    }

                    if (HitInfo.collider.GetComponent<DropOffPoint>() != null)
                    {
                        Transform resource = HitInfo.transform;

                        if (!dropOffPointsFound.Contains(resource))
                        {
                            dropOffPointsFound.Add(resource);
                        }
                    }

                    if (HitInfo.collider.GetComponent<Enemy>() != null)
                    {
                        Debug.DrawLine(transform.position, HitInfo.point, Color.red);
                        Transform enemy = HitInfo.transform;

                        if (!enemyInSight.Contains(enemy))
                        {
                            enemyInSight.Add(enemy);
                        }
                    }
                }
            }
        }
    }
}
