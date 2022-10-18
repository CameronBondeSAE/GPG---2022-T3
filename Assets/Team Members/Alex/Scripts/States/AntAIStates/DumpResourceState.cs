using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class DumpResourceState : AntAIState
    {
        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;

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
         /*   if (hasResource)
            {
                Vector3 forwards = transform.forward;
                Vector3 towardDropOffPoint = dropOffPoints[0].transform.position - rb.transform.position;
                float angle = Vector3.SignedAngle(forwards, towardDropOffPoint, Vector3.up);
                if (dropOffPoints == null) ;
                rb.AddTorque(new Vector3(0, angle * turnSpeed, 0));
            }
         */   
            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}
