using System;
using System.Collections;
using System.Collections.Generic;
using Oscar;
using UnityEngine;

namespace Marcus
{
    public class PlantBase : MonoBehaviour, IFlammable, IResource
    {
        public FlammableComponent fireness;

        private void OnEnable()
        {
            fireness.SetOnFireEvent += SetOnFire;
        }

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
            // Maybe make a thing to smoke
        }

        void SetOnFire()
        {
            print("OUCHIES FIRE HURT");
            Destroy(gameObject);
            // Combust and shrivel
        }
    }
}
