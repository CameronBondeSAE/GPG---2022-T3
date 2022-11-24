using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class FindHealthState : StateBase
    {

        public GameObject food;
        public TurnTowards turnTowards;
        public Wander wander;

        public void OnEnable()
        {
            turnTowards.targetTransform = food.transform;
            turnTowards.enabled = true;
            wander.enabled = true;
        }

        public void OnDisable()
        {
            turnTowards.enabled = false;
            wander.enabled = false;
        }
    }
}
