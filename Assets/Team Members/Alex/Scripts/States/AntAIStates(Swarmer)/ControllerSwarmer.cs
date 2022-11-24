using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Lloyd;

namespace Alex
{
    public class ControllerSwarmer : MonoBehaviour
    {
        public bool canAttack = false;
        public bool canSwarm = false;
        
        public Transform target;
        public Controller myOwnerAlienAI;

        Health health;

        private void Awake()
        {
            canAttack = false;
            canSwarm = true;
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
