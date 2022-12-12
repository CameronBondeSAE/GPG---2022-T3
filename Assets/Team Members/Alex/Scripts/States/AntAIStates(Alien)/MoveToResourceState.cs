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
        FollowPath followPath;
        TurnTowards turnTowards;
        public Shader eleShader;
        Movement movement;


        Rigidbody rb;

        public override void Create(GameObject aGameObject)
        {
            
            base.Create(aGameObject);

            owner = aGameObject;
            controller = aGameObject.GetComponent<Controller>();
            vision = aGameObject.GetComponent<Vision>();
            rb = aGameObject.GetComponent<Rigidbody>();
            followPath = aGameObject.GetComponent<FollowPath>();
            turnTowards = aGameObject.GetComponent<TurnTowards>();
            movement = aGameObject.GetComponent<Movement>();
        }
        public override void Enter()
        {
            base.Enter();

            followPath.useTurnTowards = true;
            
            controller.rend.material.shader = eleShader;
            //movement.enabled = true;
           //renderer.material.shader = eleShader;
           

           turnTowards.enabled = true;
           followPath.enabled = true;
           
            
            followPath.PathEndReachedEvent += FollowPathOnPathEndReachedEvent;
            if (vision.resourcesInSight.Count == 0 && vision.resourcesInSight != null) return;
            
            if (vision.resourcesInSight.Count > 0)
            {
                followPath.ActivatePathToTarget(vision.resourcesInSight[0].transform.position);
                vision.resourcesInSight.RemoveAll(transformToTest => transformToTest == null);
            }

           
        }

        private void FollowPathOnPathEndReachedEvent()
        {
            followPath.enabled = false;
            turnTowards.enabled = false;
            Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
        }
        
        public override void Exit()
        {
            base.Exit();
            controller.rend.material.shader = controller.defaultShader;
            followPath.PathEndReachedEvent -= FollowPathOnPathEndReachedEvent;
            
        }
    }
}