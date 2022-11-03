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
        private Color colRed = new Color32(255,0,0,255);
        private Vector3 scaleIncrease = new Vector3(3, 3, 3);
        public event Action AlmostExplode;
        //used IEnumerator so it will start on start but will wait a few seconds before continuing.
        IEnumerator Start()
        {
            AlmostExplode?.Invoke();
            //play hiss sound 

            //pulse its size before exploding
            transform.DOShakeScale(3f,
                new Vector3(.1f,.1f,.1f), 5,5f,false);
            
            //change colour to red gradually
            GetComponent<Renderer>().material.DOColor(colRed, 2f);
            
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

