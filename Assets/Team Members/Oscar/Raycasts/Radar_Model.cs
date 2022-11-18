using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace Oscar
{
    public class Radar_Model : ImmediateModeShapeDrawer, IFlammable,IPickupable
    {
        //is it on the player or not
        private bool radarOn = false;

        //for the actual raycast
        public float timer;
        private float radarSpeed = 100f;
        public Vector3 dir;

        private RaycastHit hitInfo;
        
        public LayerMask pingLayer;
        private float length = 10f;

        void Update()
        {
            if (radarOn == true)
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
                    if (hit.collider.GetComponent<IAffectedByRadar>() != null)
                    {
                        hit.collider.GetComponent<IAffectedByRadar>().Detection();
                    }
                }
            }
        }
        
        //Interfaces that Interact with this item.
        public bool isHeld { get; set; }
        public bool locked { get; set; }
        public bool autoPickup { get; set; }

        public void pickedUp100()
        {
            RadarSwitch();
        }

        public bool RadarSwitch()
        {
            if (radarOn == true)
            {
                radarOn = false;
            }
            else
            {
                radarOn = true;
            }
            return radarOn;
        }


        public void NotOn()
        {
            RadarSwitch();
        }

        public void PickedUp(GameObject interactor)
        {
            RadarSwitch();
        }

        public void PutDown(GameObject interactor)
        {
            RadarSwitch();
        }

        public void DestroySelf()
        {
            
        }
        
        public void ChangeHeat(IHeatSource heatSource, float x)
        {
            
        }
    }
}

