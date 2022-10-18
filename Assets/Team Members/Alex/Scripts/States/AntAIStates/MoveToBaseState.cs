using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Alex
{

    public class MoveToBaseState : AntAIState
    {
        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;
        Controller controller;
        Vision vision;
        private Rigidbody rb;

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
            
            if (vision.resourcesInSight.Count == 0 && vision.resourcesInSight != null) return;
            Vector3 forwards = transform.forward;
            Vector3 towardResource = vision.resourcesInSight[0].position - rb.transform.position;
            
            float angle = Vector3.SignedAngle(forwards, towardResource, Vector3.up);
            rb.AddTorque(new Vector3(0, angle * controller.turnSpeed, 0));
            
            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}