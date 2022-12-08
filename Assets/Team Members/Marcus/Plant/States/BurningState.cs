using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Marcus
{
    public class BurningState : NetworkBehaviour
    {
        public MonoBehaviour growthState;
        public MonoBehaviour dyingState;
        
        public Flammable cooling;
        public Health health;

        private float waitTimer = 3f;

        private void OnEnable()
        {
            // cooling.CoolDown += ExtinguishFlame;
        }

        // Not Networked yet
        void ExtinguishFlame()
        {
            if (health.HP < 50)
            {
                StartCoroutine(ChangeState(dyingState));
            }
            else
            {
                StartCoroutine(ChangeState(growthState));
            }
        }

        IEnumerator ChangeState(MonoBehaviour destinationState)
        {
            yield return new WaitForSeconds(waitTimer);
            GetComponent<StateManager>().ChangeState(destinationState);
        }
    }
}
