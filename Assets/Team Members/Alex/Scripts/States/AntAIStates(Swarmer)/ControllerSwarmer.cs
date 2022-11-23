using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Alex
{
    public class ControllerSwarmer : MonoBehaviour
    {
        public bool isSwarmingAgain;
        public bool isAttackingTarget = false;
        public bool canAttack = false;
        public bool canSwarm = false;

       
        public Neighbours neighbours;
        public Transform target;

        private void Awake()
        {
            canAttack = false;
            isAttackingTarget = false;
            isSwarmingAgain = true;
            canSwarm = true;
        }
            

        public bool IsAttacking()
        {
            if(canAttack) 
                return isAttackingTarget;
            else
            {
                return false;
            }
            
            /*
            if (neighbours.GetComponent<MoveToEnemyState>().canSwarm) ;
            {
                return isAttacking;
            }
            */
        }

        public bool IsSwarming()
        {
            if(canSwarm) 
                return isSwarmingAgain;
            else
            {
                return false;
            }
        }
    }
}
