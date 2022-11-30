using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marcus
{
    public class BurningState : MonoBehaviour
    {
        public MonoBehaviour matureState;
        public MonoBehaviour dyingState;
        
        public Flammable cooling;
        public Health health;

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
            if (health.HP < 50)
            {
                GetComponent<StateManager>().ChangeState(dyingState);
            }
            else
            {
                GetComponent<StateManager>().ChangeState(matureState);
            }
        }
    }
}
