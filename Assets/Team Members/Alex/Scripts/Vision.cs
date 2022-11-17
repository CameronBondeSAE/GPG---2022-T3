using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Shapes;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Linq;

namespace Alex
{

    public class Vision : MonoBehaviour
    {
        public int rays = 10;
        public float spacingScale = 1f;
        public List<Transform> resourcesInSight;
        public List<Transform> enemyInSight;
        public List<Transform> dropOffPointsFound;
        public Array[] arrayOfThingsHit;
        public TestShapes testShapes;



        private void FixedUpdate()
        {
            
            //enemyInSight.Clear();
            //resourcesInSight.Clear();
            //dropOffPointsFound.Clear();



            testShapes.polygonPath.ClearAllPoints();
            
            //converting the Y space from X coordinates to get flat vision cone
            testShapes.polygonPath.AddPoint(transform.position.x, transform.position.z);
            
            
            
            for (int i = -rays; i < rays; i++)
            {
                
                // Very simple. Doesn't take any tilting or pitching into account, but is fine for horizontal only AIs
                Vector3 dir = Quaternion.Euler(0, i*spacingScale, 0) * transform.forward;
    
                Debug.DrawRay(transform.position, dir * 10f, Color.green);

                Physics.Raycast(transform.position, dir, out RaycastHit HitInfo);
                if(HitInfo.collider == null) continue;

                
                
                testShapes.polygonPath.AddPoint(HitInfo.point.x,HitInfo.point.z);
                // CAM BIT
                // Add point for later rendering
                // testShapesViewModel.polygonPath.AddPoint(new Vector2()); // X and Z from ray
                
                if (HitInfo.collider.GetComponent<Target>() != null)
                {
                    //Debug.DrawLine(transform.position, HitInfo.point, Color.red);
                    Transform enemy = HitInfo.transform;

                    if (!enemyInSight.Contains(enemy))
                    {
                        enemyInSight.Add(enemy);
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
            
            
            //Sorting all the lists so that the closest will be first in order for AStar to use the closest object. 
            resourcesInSight = resourcesInSight.OrderBy(
                resource => Vector3.Distance(this.transform.position,resource.transform.position)
            ).ToList();
            
            enemyInSight = enemyInSight.OrderBy(
                enemy => Vector3.Distance(this.transform.position,enemy.transform.position)
            ).ToList();
            
            dropOffPointsFound = dropOffPointsFound.OrderBy(
                resource => Vector3.Distance(this.transform.position,resource.transform.position)
            ).ToList();
            
            
            testShapes.polygonPath.AddPoint(transform.position.x, transform.position.z);
            
        }
        // CAM BIT
        
        // Add last point which is the position of the player
    }
}
