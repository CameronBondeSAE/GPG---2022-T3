using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marcus
{
    public class PlantBase : MonoBehaviour, IFlammable, IPickupable
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public void ChangeHeat(IHeatSource heatSource, float x)
        {
            //Do dying things
        }

        public void PickedUp(GameObject interactor)
        {
            //Do pickup things
        }

        public void PutDown(GameObject interactor)
        {
            //Just exist?
        }

        public void DestroySelf()
        {
            throw new System.NotImplementedException();
        }

        public bool isHeld { get; set; }
        public bool locked { get; set; }
        public bool autoPickup { get; set; }
    }
}
