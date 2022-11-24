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

        private Flammable flammable;
        
        public void ChangeHeat(IHeatSource heatSource, float x)
        {
            SetOnFire();
        }
        public void SetOnFire()
        {
            stateManager.ChangeState(nearlyExplode);
        }
    }
}
