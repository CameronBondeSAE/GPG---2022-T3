using System;
using System.Collections;
using System.Collections.Generic;
using Oscar;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

namespace Oscar
{
    public class Explosive_View : NetworkBehaviour
    {
        public ParticleSystem Smoke;
        public Oscar.ExplosiveNearlyExplodeState AlmostBlowUp;
        
        private Color colRed = new Color32(255,0,0,255);
        
        private void OnEnable()
        {
            AlmostBlowUp.AlmostExplode += ItCouldExplode;
        }
        
        
        public void ItCouldExplode()
        {
            Smoke.Play();
            //play hiss sound 

            //pulse its size before exploding
            transform.DOShakeScale(3f,
                new Vector3(.1f,.1f,.1f), 5,5f,false);
            
            //change colour to red gradually
            GetComponent<Renderer>().material.DOColor(colRed, 3f);
        }
    
    }

}
