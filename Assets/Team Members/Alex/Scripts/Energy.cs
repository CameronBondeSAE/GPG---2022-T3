using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Alex
{
    public class Energy : MonoBehaviour
    {
        public float energyAmount;
        public float energyMax = 100f;
        public float energyMin = 0f;


        // Start is called before the first frame update
        void Start()
        {

            
        }

        // Update is called once per frame
        void Update()
        {
            if (energyAmount >= energyMax)
                energyAmount = energyMax;

            if (energyAmount <= energyMin)
                energyAmount = energyMin;
        }
    }
}

