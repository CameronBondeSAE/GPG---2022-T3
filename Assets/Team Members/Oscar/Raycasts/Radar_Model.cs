using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace Oscar
{
    public class Radar_Model : ImmediateModeShapeDrawer, IFlammable,IPickupable
    {
        //for the actual raycast
        public float timer;
        private float radarSpeed = 100f;
        public Vector3 dir;

        private RaycastHit hitInfo;
        
        public LayerMask pingLayer;
        private float length = 10f;

        void Update()
        {
            //create the loop for the radar using time.deltatime
            timer += Time.deltaTime * radarSpeed;
            if (timer >= 360f)
            {
                timer = 0f;
            }
            
            //defined direction over time
            dir = Quaternion.Euler(0, timer, 0) * transform.forward * length;
            
            //the actual raycast that will read the collisions if there are any
            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, length,pingLayer))
            {
                if (hit.collider != null)
                {
                    
                }
            }
        }
        
        //Interfaces that Interact with this item.
        public bool isHeld { get; set; }
        public bool locked { get; set; }
        public bool autoPickup { get; set; }

        public void PickedUp(GameObject interactor)
        {
            throw new System.NotImplementedException();
        }

        public void PutDown(GameObject interactor)
        {
            throw new System.NotImplementedException();
        }
        
        public void DestroySelf()
        {
            throw new System.NotImplementedException();
        }
        
        public void ChangeHeat(IHeatSource heatSource, float x)
        {
            
        }
    }
}

