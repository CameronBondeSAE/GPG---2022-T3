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

        

        private Rigidbody rb;

        public override void Create(GameObject aGameObject)
        {
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
            
            if (vision.resourcesInSight.Count == 0) return;
            sensor.MoveToResource();
            
            /*
            Vector3 forwards = transform.forward;
            Vector3 towardResource = vision.resourcesInSight[0].position - rb.transform.position ;
            float angle = Vector3.SignedAngle(forwards, towardResource, Vector3.up);
            if (vision.resourcesInSight == null) ;
            rb.AddTorque(new Vector3(0,angle * turnSpeed,0));
            */
            
            Finish();
        }
    }
}
