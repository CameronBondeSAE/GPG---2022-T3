using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Linq;

namespace Alex
{

    public class Vision : MonoBehaviour
    {
        public int rays = 10;
        public float spacingScale = 1f;
        public float heightOffset = -0.5f;
        public List<Transform> resourcesInSight;
        public List<Transform> enemyInSight;
        public List<Transform> dropOffPointsFound;
        public List<Transform> thingsToRemove;
        public Array[] arrayOfThingsHit;
        public TestShapes testShapes;
        public LayerMask layerMask;

        public bool affectsOnScreenVisibility = false;


        private void FixedUpdate()
        {
            
            //enemyInSight.Clear();
            //resourcesInSight.Clear();
            //dropOffPointsFound.Clear();



            testShapes.polygonPath.ClearAllPoints();
            
            //converting the Y space from X coordinates to get flat vision cone
            testShapes.polygonPath.AddPoint(transform.position.x, transform.position.z);


            Vector3 offset = new Vector3(0, heightOffset, 0);
            
            for (int i = -rays; i < rays; i++)
            {
                
                // Very simple. Doesn't take any tilting or pitching into account, but is fine for horizontal only AIs
                Vector3 dir = Quaternion.Euler(0, i*spacingScale, 0) * transform.forward;
    
                //Physics.Raycast(transform.position, dir, out RaycastHit HitInfo);
                
                Physics.Raycast(transform.position + offset, dir, out RaycastHit HitInfo, 999f, layerMask, QueryTriggerInteraction.Collide);
                
                if(HitInfo.collider == null) continue;


                if (affectsOnScreenVisibility)
                {
	                IAffectedByVisibility[] affectedByVisibilities = HitInfo.collider.GetComponents<IAffectedByVisibility>();
	                if (affectedByVisibilities != null)
	                {
		                foreach (var visibility in affectedByVisibilities)
		                {
			                visibility.Detection(1);
		                }
	                }
                }

                
                testShapes.polygonPath.AddPoint(HitInfo.point.x,HitInfo.point.z);
                // CAM BIT
                // Add point for later rendering
                // testShapesViewModel.polygonPath.AddPoint(new Vector2()); // X and Z from ray
                
                if (HitInfo.collider.GetComponent<Target>() != null)
                {
                    Debug.DrawLine(transform.position, HitInfo.point, Color.red);
                    Transform enemy = HitInfo.transform;

                    if (!enemyInSight.Contains(enemy))
                    {
                        enemyInSight.Add(enemy);
                    }
                }
                
                else
                {
                    Debug.DrawLine(transform.position, HitInfo.point, Color.green);

                    if (HitInfo.collider.GetComponent<IResource>() != null)
                    {

                        Transform resource = HitInfo.transform;

                        if (!resourcesInSight.Contains(resource))
                        {
                            resourcesInSight.Add(resource);
                        }
                    }
                    else if (HitInfo.collider.GetComponent<Checkpoint>() != null)
                    {
                        Transform hq = HitInfo.transform;

                        if (!dropOffPointsFound.Contains(hq))
                        {
                            dropOffPointsFound.Add(hq);
                        }
                    } 
                    else if (HitInfo.collider.GetComponent<Enemy>() != null)
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

            if (enemyInSight.Count > 0)
            {
                if (enemyInSight[0] == null)
                {
                    enemyInSight.Remove(enemyInSight[0]);
                }
            }

            /*
            List<int> thingsToRemove = new List<int>();
            for (var index = 0; index < (resourcesInSight).Count; index++)
            {
                var resource = resourcesInSight[index];
                if(resource == null)
                    thingsToRemove.Add(index);
            }
            */
            
            //Sorting all the lists so that the closest will be first in order for AStar to use the closest object. 
            if (resourcesInSight == null) return;
            resourcesInSight.RemoveAll(transformToTest => transformToTest == null);
            
            /*
            foreach (Transform resource in (resourcesInSight))
            {
                if (resource == null)
                    resourcesInSight.Remove(resource);
            }
            */
            if(resourcesInSight.Count > 1)
                resourcesInSight = resourcesInSight.OrderBy(resource => Vector3.Distance(this.transform.position,resource.transform.position)).ToList();
            
            if(enemyInSight.Count > 1)
                enemyInSight = enemyInSight.OrderBy(enemy => Vector3.Distance(this.transform.position,enemy.transform.position)).ToList();

            if(dropOffPointsFound.Count > 1)
                dropOffPointsFound = dropOffPointsFound.OrderBy(resource => Vector3.Distance(this.transform.position,resource.transform.position)).ToList();
            
            
            if(testShapes.polygonPath.Count > 2) // CAM: I think when it starts above walls, it doesn't set the points as no raycast hit anything
	            testShapes.polygonPath.AddPoint(transform.position.x, transform.position.z);
            
        }
        // CAM BIT
        
        // Add last point which is the position of the player
    }
}
