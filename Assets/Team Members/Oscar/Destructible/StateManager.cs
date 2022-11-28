using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Oscar
{
    public class StateManager : MonoBehaviour
    {
        //states for the switching of states
        public MonoBehaviour startingState;
        public MonoBehaviour currentState;

        public Flammable flammable;

        // Start is called before the first frame update
        void Start()
        {
            ChangeState(startingState);
        }

        public void ChangeState(MonoBehaviour newState)
        {
            //if the state is already the same state dont change
            if (newState == currentState)
            {
                return;
            }

            //if the new stat is not the same as the previous state then disable the previous state
            if (currentState != null)
            {
                currentState.enabled = false;
            }
            //enable the state selected
            newState.enabled = true;
            //new state becomes the current state
            currentState = newState;
        }
        
    }
}

