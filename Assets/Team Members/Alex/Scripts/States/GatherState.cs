using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class GatherState : StateBase
    {
        public TurnTowards turnTowards;
        public GameObject target;
        public Movement movement;
        
        
        // Start is called before the first frame update

        public void OnEnable()
        {
            movement.enabled = true;
            turnTowards.enabled = true;
             
        }
        
        public void OnDisable()
        {
            turnTowards.enabled = false;
        }
    }
}
