using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class MoveToEnemyState : AntAIState
    {
        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;
        [SerializeField]
        Vision vision;
        Rigidbody rb;
        Controller controller;
        Renderer renderer;
        public Shader agroShader;
        FollowPath followPath;
        TurnTowards turnTowards;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            owner = aGameObject;
            controller = aGameObject.GetComponent<Controller>();
            vision = aGameObject.GetComponent<Vision>();
            rb = aGameObject.GetComponent<Rigidbody>();
            renderer = aGameObject.GetComponent<Renderer>();
            turnTowards = aGameObject.GetComponent<TurnTowards>();
            followPath = aGameObject.GetComponent<FollowPath>();
        }
        public override void Enter()
        {
            base.Enter();
            controller.renderer.material.shader = agroShader;
            

            turnTowards.enabled = true;
            followPath.enabled = true;
            
            
            followPath.PathEndReachedEvent += FollowPathOnPathEndReachedEvent;
            
            //Exit early if no enemies in sight
            if (vision.enemyInSight.Count == 0 && vision.enemyInSight != null) return;
            if (vision.enemyInSight.Count > 0)
            {
                followPath.ActivatePathToTarget(vision.enemyInSight[0].transform.position);
            }

           
        }

        private void FollowPathOnPathEndReachedEvent()
        {
            followPath.enabled = false;
            turnTowards.enabled = false;
            Finish();
        }

        /*
        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            
            if (vision.enemyInSight.Count == 0 && vision.enemyInSight != null) return;
            if (vision.enemyInSight.Count > 0)
            {
                owner.GetComponent<TurnTowards>().targetPosition = vision.enemyInSight[0].transform.position;
                //owner.GetComponent<TurnTowards>().targetTransform = vision.resourcesInSight[0].transform.position;
                Finish();
            }
        }
        */
        public override void Exit()
        {
            base.Exit();
            controller.renderer.material.shader = controller.defaultShader;
            followPath.PathEndReachedEvent -= FollowPathOnPathEndReachedEvent;
            
        }
    }
}
