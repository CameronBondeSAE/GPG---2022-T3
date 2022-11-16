using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marcus
{
    public class PlantBase : MonoBehaviour, IFlammable
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
    }
}
