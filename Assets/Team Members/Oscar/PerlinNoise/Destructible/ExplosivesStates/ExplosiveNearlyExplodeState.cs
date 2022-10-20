using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveNearlyExplodeState : MonoBehaviour
    {
        private Color render;
        private bool exploding = false;
        private Color explodeRed = Color.red;
        
        private void Start()
        {
            ChangeColour();
        }

        void ChangeColour()
        {
            if (exploding == false)
            {
                GetComponent<Renderer>().material.color = explodeRed;
            }
        }

        private void Update()
        {
            //pulse the object
            //tween this
            
            //google tweening
        }
        
        
        
    }
}

