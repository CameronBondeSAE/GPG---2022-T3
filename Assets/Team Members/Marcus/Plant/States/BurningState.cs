using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marcus
{
    public class BurningState : MonoBehaviour
    {
        public MonoBehaviour growthState;
        public MonoBehaviour dyingState;
        
        public Flammable cooling;
        public Health health;

        private float waitTimer = 3f;

        private void OnEnable()
        {
            cooling.CoolDown += ExtinguishFlame;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void ExtinguishFlame()
        {
            if (health.HP.Value < 50)
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
