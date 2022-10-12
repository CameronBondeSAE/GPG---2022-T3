using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class SearchResourceState : AntAIState
    {
        public GameObject target;

        

        public override void Enter()
        {
            base.Enter();

            target = FindObjectOfType<Resource>().gameObject;
            
            Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}
