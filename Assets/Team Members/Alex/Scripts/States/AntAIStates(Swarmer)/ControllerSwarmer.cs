using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Lloyd;
using Unity.Netcode;

namespace Alex
{
    public class ControllerSwarmer : MonoBehaviour
    {
        public bool canAttack = false;
        public bool canSwarm = false;
        
        public Transform target;
        public Controller myOwnerAlienAI;

        public Health health;

        private void Awake()
        {
            canAttack = false;
            canSwarm = true;
            
            health.YouDied += HealthOnYouDied;
        }

        private void HealthOnYouDied(GameObject obj)
        {
	        if (NetworkManager.Singleton.IsServer)
	        {
		        Destroy(gameObject);
                Debug.Log("Swarmer died");
	        }
        }


        public bool IsAttacking()
        {
            return canAttack;
        }

        public bool IsSwarming()
        {
            return canSwarm;
        }

        public bool HurtEnemy()
        {
            return false;
        }
    }
}
