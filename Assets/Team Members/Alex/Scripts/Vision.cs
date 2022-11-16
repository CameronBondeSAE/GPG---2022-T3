using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
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
        public Array[] arrayOfThingsHit;
        public TestShapes testShapes;



        private void FixedUpdate()
        {
            
            //enemyInSight.Clear();
            //resourcesInSight.Clear();
            //dropOffPointsFound.Clear();
            
            testShapes.polygonPath.ClearAllPoints();

            testShapes.polygonPath.AddPoint(new Vector2(transform.position.x + testShapes.transform.InverseTransformPoint(Vector3.zero).x, transform.position.z + testShapes.transform.InverseTransformPoint(Vector3.zero).z));
            
            
            
            for (int i = -rays; i < rays; i++)
            {
                
                // Very simple. Doesn't take any tilting or pitching into account, but is fine for horizontal only AIs
                Vector3 dir = Quaternion.Euler(0, i*spacingScale, 0) * transform.forward;
    
                Debug.DrawRay(transform.position, dir * 10f, Color.green);

                Physics.Raycast(transform.position, dir, out RaycastHit HitInfo);
                if(HitInfo.collider == null) continue;

                
                
                testShapes.polygonPath.AddPoints(new Vector2(HitInfo.point.x +  testShapes.transform.InverseTransformPoint(Vector3.zero).x, HitInfo.point.z + testShapes.transform.InverseTransformPoint(Vector3.zero).z));
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
            testShapes.polygonPath.AddPoint(new Vector2(transform.position.x + testShapes.transform.InverseTransformPoint(Vector3.zero).x, transform.position.z + testShapes.transform.InverseTransformPoint(Vector3.zero).z));
        }
        // CAM BIT
        
        // Add last point which is the position of the player
    }
}
