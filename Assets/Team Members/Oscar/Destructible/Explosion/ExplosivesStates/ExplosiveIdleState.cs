using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveIdleState : MonoBehaviour, IFlammable
    {
        //effected by fire so needs to respond to being hit by fire.
        public void SetOnFire()
        {
            GetComponent<Oscar.StateManager>().ChangeState(GetComponent<ExplosiveNearlyExplodeState>());
        }

        
        private void OnDisable()
        {
            
        }
    }
}
