 using System;
using System.Collections;
using System.Collections.Generic;
 using Unity.Netcode;
 using Unity.VisualScripting;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveIdleState : MonoBehaviour
    {
        public Oscar.StateManager stateManager;
        public MonoBehaviour nearlyExplode;
        //effected by fire so needs to respond to being hit by fire.

        private Flammable flammable;
        
        public void OnEnable()
        {
	        if (!NetworkManager.Singleton.IsServer) return;
	        flammable = GetComponent<Flammable>();
	        flammable.SetOnFireEvent += SetOnFire;
        }
        
        private void SetOnFire()
        {
            stateManager.ChangeState(nearlyExplode);
        }
    }
}
