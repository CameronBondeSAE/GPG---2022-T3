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
        
        private void Start()
        {
            StartCoroutine(ChangeColour());
        }

        IEnumerator ChangeColour()
        {
            GetComponent<Renderer>().material.color = explodeRed;
            yield return new WaitForSeconds(3);
            GetComponent<Oscar.StateManager>().ChangeState(GetComponent<ExplosiveExplodeState>());

        }

        private void OnDisable()
        {
            
        }
    }
}

