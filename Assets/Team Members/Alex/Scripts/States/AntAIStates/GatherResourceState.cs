using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class GatherResourceState : AntAIState
    {
        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;
        [SerializeField]
        Vision vision;

        public float turnSpeed = 1;
        private Rigidbody rb;
        Controller controller;

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

            //Vector3 myPosition = transform.position;
            //Vector3 targetPosition = vision.resourcesInSight[0].position;

            Vector3 myPosition = transform.position;
            Vector3 targetPosition = vision.resourcesInSight[0].position;


            if (Vector3.Distance(myPosition, targetPosition) < 0.2f)
                controller.inventory.resources += 1;
            vision.resourcesInSight.Remove(transform);
            if (controller.inventory.capacityReached)
                controller.hasResource = true;
            
                
            
            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}
