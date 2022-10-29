using System;
using System.Collections;
using System.Collections.Generic;
using Oscar;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveSmoke : MonoBehaviour
    {
        public ParticleSystem Smoke;
        public Oscar.ExplosiveNearlyExplodeState AlmostBlowUp;
        private void OnEnable()
        {
            AlmostBlowUp.AlmostExplode += SmokeDispense;
        }
    
        public void SmokeDispense()
        {
            Smoke.Play();
        }
    
    }

}
