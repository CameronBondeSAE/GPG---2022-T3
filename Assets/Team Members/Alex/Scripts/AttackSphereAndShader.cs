using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;

namespace Alex
{
    public class AttackSphereAndShader : MonoBehaviour
    {
        public GameObject attackCollider;
        public Transform myTarget;
        public ControllerSwarmer controllerSwarmer;
        public Health enemyHealth;
        private float damageOverTime;
        private float damagePerSecond = 10;
        

        public void OnEnable()
        {
            myTarget = controllerSwarmer.target;
            if (myTarget != null) enemyHealth = myTarget.GetComponent<Health>();
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.transform == myTarget) 
                DealDamage();
        }

        void DealDamage()
        {
            damageOverTime = damagePerSecond * Time.deltaTime;
            if(enemyHealth != null)
                enemyHealth.ChangeHP(-damageOverTime);
        }
        
    }
}
