using System;
using UnityEngine;

namespace Oscar
{
    public class Radar_Model : MonoBehaviour, IFlammable,IPickupable
    {
        //is it on the player or not
        private bool radarOn = false;

        //for the actual raycast
        public float timer;
        private float radarSpeed = 100f;
        public Vector3 dir;

        public Ray ray;

        public LayerMask pingLayer;
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
            theDir = dir;

            if (radarOn == true)
            {
                //the actual raycast that will read the collisions if there are any
                ray = new Ray(transform.position, transform.forward + dir);
                RaycastHit hitinfo = new RaycastHit();
            
                if (Physics.Raycast(ray,out hitinfo, length))
                {
                    if (hitinfo.collider.GetComponent<IAffectedByRadar>() != null)
                    {
                        hitinfo.collider.GetComponent<IAffectedByRadar>().Detection();
                    }
                }
            }
        }

        public Vector3 theDir
        {
            get { return dir; }
            set { dir = value; }
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

        public void PickedUp(GameObject interactor, ulong localClientId)
        {
            RadarSwitchOn();
        }

        public void PutDown(GameObject interactor, ulong localClientId)
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

