using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class ChaseState : StateBase
    {
        public TurnTowards turnTowards;
        public GameObject target;
        
        // Start is called before the first frame update

        public void OnEnable()
        {
            turnTowards.target = target.transform;
            turnTowards.enabled = true;
        }
        
        public void OnDisable()
        {
            turnTowards.enabled = false;
        }
    }
}

