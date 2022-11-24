using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;

namespace Alex
{
    public class AttackSphereAndShader : MonoBehaviour
    {
        public GameObject gameObject;
        public Transform myTarget;
        public ControllerSwarmer controllerSwarmer;
        public Health enemyHealth;


        public void OnEnable()
        {
            myTarget = controllerSwarmer.target;
            enemyHealth = myTarget.GetComponent<Health>();
        }

        public void OnCollisionEnter()
        {
            if (myTarget)
                enemyHealth.ChangeHP(-10f);
        }
    }
}
