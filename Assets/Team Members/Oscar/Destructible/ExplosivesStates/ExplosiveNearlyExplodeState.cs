using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveNearlyExplodeState : MonoBehaviour
    {
        private Color render;
        private Color explodeRed = Color.red;

        //used IEnumerator so it will start on start but will wait a few seconds before continuing.
        IEnumerator Start()
        {
            //play hiss sound 
            
            GetComponent<Renderer>().material.color = explodeRed;
            yield return new WaitForSeconds(3);
            GetComponent<Oscar.StateManager>().ChangeState(GetComponent<ExplosiveExplodeState>());
        }

        private void OnDisable()
        {
            GetComponent<ExplosiveRaycast>().ExplosionRaycast();
            //play explosion sound 
        }
    }
}

