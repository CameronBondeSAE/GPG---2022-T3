using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class MoveToResourceState : AntAIState
    {
        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;
        [SerializeField]
        Vision vision;
        public float turnSpeed = 1;
        Resource resource;
        Controller controller;

        

        private Rigidbody rb;

        public override void Create(GameObject aGameObject)
        {
            controller = aGameObject.GetComponent<Controller>();
            base.Create(aGameObject);

            owner = aGameObject;
            vision = aGameObject.GetComponent<Vision>();
            rb = aGameObject.GetComponent<Rigidbody>();
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
    }
}
