using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveNearlyExplodeState : MonoBehaviour
    {
        private Material myMAT;

        public event Action AlmostExplode;
        //used IEnumerator so it will start on start but will wait a few seconds before continuing.
        IEnumerator Start()
        {
            AlmostExplode?.Invoke();
            //play hiss sound 

            DOTween.To(setter, 0, 1, 3);
            //GetComponent<Renderer>().material.color = explodeRed;
            yield return new WaitForSeconds(3);
            GetComponent<Oscar.StateManager>().ChangeState(GetComponent<ExplosiveExplodeState>());
        }

        private void setter(float pNewValue)
        {
            
        }

        private void OnDisable()
        {
            GetComponent<ExplosiveRaycast>().ExplosionRaycast();
            //play explosion sound 
        }
    }
}

