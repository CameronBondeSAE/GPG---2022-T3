using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oscar
{
    public class Radar_Model : MonoBehaviour, IFlammable
    {
        //for the actual raycast
        private float timer;
        private float radarSpeed = 100f;
        private Vector3 dir;

        private RaycastHit hitInfo;
        
        public LayerMask pingLayer;
        public float length = 3f;

        
        
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

            if (Physics.Raycast(ray, out hit, length))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.layer == pingLayer)
                    {
                        print("ping");
                    }
                }
            }
        }
        
        public bool isHeld { get; set; }
        public bool locked { get; set; }
        
        public void PickedUp(GameObject interactor)
        {
            throw new System.NotImplementedException();
        }

        public void PutDown(GameObject interactor)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeHeat(IHeatSource heatSource, float x)
        {
            
        }
    }
}

