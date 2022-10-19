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
        public Wonder wonder;

        public void OnEnable()
        {
            turnTowards.targetTransform = food.transform;
            turnTowards.enabled = true;
            wonder.enabled = true;
        }

        public void OnDisable()
        {
            turnTowards.enabled = false;
            wonder.enabled = false;
        }
    }
}
