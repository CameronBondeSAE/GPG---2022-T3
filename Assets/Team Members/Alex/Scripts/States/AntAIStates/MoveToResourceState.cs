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
            
            base.Create(aGameObject);

            owner = aGameObject;
            controller = aGameObject.GetComponent<Controller>();
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
            if (vision.resourcesInSight.Count > 0)
            {
                owner.GetComponent<TurnTowards>().targetPosition = vision.resourcesInSight[0].transform.position;
                //owner.GetComponent<TurnTowards>().targetTransform = vision.resourcesInSight[0].transform.position;
                Finish();
            }
        }
    }
}

