 using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveIdleState : MonoBehaviour, IFlammable
    {
        public Oscar.StateManager stateManager;
        public MonoBehaviour nearlyExplode;
        //effected by fire so needs to respond to being hit by fire.

        private void OnEnable()
        {
            stateManager.flammable.SetOnFireEvent += SetOnFire;
        }
        
        public void ChangeHeat(IHeatSource heatSource, float x)
        {
            stateManager.ChangeState(nearlyExplode);

            SetOnFire();
        }
        public void SetOnFire()
        {
            stateManager.ChangeState(nearlyExplode);
        }
    }
}
