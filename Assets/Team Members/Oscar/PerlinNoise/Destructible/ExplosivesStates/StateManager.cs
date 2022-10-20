using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Oscar
{
    public class StateManager : MonoBehaviour
    {
        public MonoBehaviour startingState;
        public MonoBehaviour currentState;
        
        // Start is called before the first frame update
        void Start()
        {
            Changestate(startingState);
        }

        private void Changestate(MonoBehaviour newState)
        {
            if (newState == currentState)
            {
                return;
            }

            if (currentState != null)
            {
                currentState.enabled = false;
            }

            newState.enabled = true;
            
            currentState = newState;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

