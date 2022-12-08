using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using Shapes;
using Unity.Netcode;
using UnityEngine;

namespace Oscar
{
    public class Radar_Model : NetworkBehaviour, IFlammable,IPickupable
    {
        //is it on the player or not
        private bool radarOn = false;

        //for the actual raycast
        public float timer;
        private float radarSpeed = 100f;
        public Vector3 dir;

        public RaycastHit hit;
        
        [SerializeField]private float length = 10f;
        
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
            if (radarOn == true)
            {
                if (Physics.Raycast(ray, out hit, length))
                {
                    if (hit.collider.GetComponent<IAffectedByRadar>() != null)
                    {
                        hit.collider.GetComponent<IAffectedByRadar>().Detection(); 
                    }
                }
            }
        }

        public event Action<bool> RadarOnNow;
        
        //Interfaces that Interact with this item.
        public bool isHeld { get; set; }
        public bool locked { get; set; }

        public bool autoPickup
        {
            get
            {
                return false;
            }
            set { }
        }
        
        public bool RadarSwitchOn()
        {
            if (radarOn == false)
            {
                radarOn = true;
            }
            RadarOnNow?.Invoke(radarOn);
            return radarOn;
        }

        public bool RadarSwitchOff()
        {
            if (radarOn == true)
            {
                radarOn = false;
            }
            RadarOnNow?.Invoke(radarOn);
            return radarOn;
        }

        public void PickedUp(GameObject interactor, ulong networkObjectId)
        {
            RadarSwitchOn();
            //TODO: OLLIE
            //copy the flamethrower PickUp/PutDown client rpcs to get this to actually be picked up
        }

        public void PutDown(GameObject interactor, ulong networkObjectId)
        {
            RadarSwitchOff();
        }

        public void DestroySelf()
        {
            
        }
        
        public void ChangeHeat(IHeatSource heatSource, float x)
        {
            
        }
    }
}

