using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class SearchResourceState : AntAIState
    {
        public GameObject target;

        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;
        Vision vision;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            owner = aGameObject;
        }

        public override void Enter()
        {
            base.Enter();

            Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            //if (vision.resourcesInSight != null)
                
            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}
