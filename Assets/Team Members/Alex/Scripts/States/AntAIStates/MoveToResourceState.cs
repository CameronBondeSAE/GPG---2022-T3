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
        Renderer renderer;
        public Shader eleShader;


        Rigidbody rb;

        public override void Create(GameObject aGameObject)
        {
            
            base.Create(aGameObject);

            owner = aGameObject;
            controller = aGameObject.GetComponent<Controller>();
            vision = aGameObject.GetComponent<Vision>();
            rb = aGameObject.GetComponent<Rigidbody>();
            followPath = aGameObject.GetComponent<FollowPath>();
            renderer = aGameObject.GetComponent<Renderer>();
        }
        public override void Enter()
        {
            base.Enter();
            controller.renderer.material.shader = eleShader;
           //renderer.material.shader = eleShader;
            
            followPath.enabled = true;
            
            followPath.PathEndReachedEvent += FollowPathOnPathEndReachedEvent;
            if (vision.resourcesInSight.Count == 0 && vision.resourcesInSight != null) return;
            if (vision.resourcesInSight.Count > 0)
            {
                followPath.ActivatePathToTarget(vision.resourcesInSight[0].transform.position);
            }

           
        }

        private void FollowPathOnPathEndReachedEvent()
        {
            followPath.enabled = false;
            
            Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);

            
        }
        
        public override void Exit()
        {
            base.Exit();
            controller.renderer.material.shader = controller.defaultShader;
            followPath.PathEndReachedEvent -= FollowPathOnPathEndReachedEvent;
            
        }
    }
}

