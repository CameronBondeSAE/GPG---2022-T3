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
        TurnTowards turnTowards;
        FollowPath followPath;
        

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            owner = aGameObject;
            controller = aGameObject.GetComponent<Controller>();
            vision = aGameObject.GetComponent<Vision>();
            rb = aGameObject.GetComponent<Rigidbody>();
            owner = aGameObject;
            turnTowards = aGameObject.GetComponent<TurnTowards>();
            followPath = aGameObject.GetComponent<FollowPath>();
        }


        public override void Enter()
        {
            base.Enter();
            
            turnTowards.enabled = true;
            followPath.enabled = true;
            
            
            followPath.PathEndReachedEvent += FollowPathOnPathEndReachedEvent;
            
            //Exit early if no enemies in sight
            if (vision.dropOffPointsFound.Count == 0 && vision.dropOffPointsFound != null) return;
            if (vision.dropOffPointsFound.Count > 0)
            {
                followPath.ActivatePathToTarget(vision.dropOffPointsFound[0].transform.position);
            }
        }
        
        
        private void FollowPathOnPathEndReachedEvent()
        {
            followPath.enabled = false;
            turnTowards.enabled = false;
            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            followPath.PathEndReachedEvent -= FollowPathOnPathEndReachedEvent;
            Finish();
        }
    }
}