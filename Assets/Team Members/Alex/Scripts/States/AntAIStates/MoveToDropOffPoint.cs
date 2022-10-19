using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Alex
{

    public class MoveToDropOffPoint : AntAIState
    {
        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;
        Controller controller;
        Vision vision;
        private Rigidbody rb;
        DropOffPoint dropOffPoint;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            owner = aGameObject;
            controller = aGameObject.GetComponent<Controller>();
            vision = aGameObject.GetComponent<Vision>();
            rb = aGameObject.GetComponent<Rigidbody>();
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
            
            if (vision.dropOffPointsFound.Count == 0 && vision.dropOffPointsFound != null) return;
            if (vision.dropOffPointsFound.Count > 0)
            {
                owner.GetComponent<TurnTowards>().targetPosition = vision.dropOffPointsFound[0].transform.position;
                Finish();
            }
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}