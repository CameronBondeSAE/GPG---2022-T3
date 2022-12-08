using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ParadoxNotion;
using Shapes;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

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
        public Shader agroShader;
        FollowPath followPath;
        TurnTowards turnTowards;
        TestShapes testShapes;
        Neighbours neighbours;
        AttackSphereAndShader attackSphereAndShader;

        // public Transform target;
        public Vector3 target;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            owner = aGameObject;
            controller = aGameObject.GetComponent<Controller>();
            vision = aGameObject.GetComponent<Vision>();
            rb = aGameObject.GetComponent<Rigidbody>();
            turnTowards = aGameObject.GetComponent<TurnTowards>();
            followPath = aGameObject.GetComponent<FollowPath>();
            testShapes = aGameObject.GetComponent<TestShapes>();
            neighbours = aGameObject.GetComponent<Neighbours>();
            attackSphereAndShader = aGameObject.GetComponent<AttackSphereAndShader>();
        }
        public override void Enter()
        {
            base.Enter();
            
            followPath.useTurnTowards = false;

            if (vision.enemyInSight.Count == 0 && vision.enemyInSight != null) return;
            
            target = vision.enemyInSight[0].position;
            
            controller.rend.material.shader = agroShader;
            testShapes.colour = Color.red;
            testShapes.intensity = 2;

            turnTowards.enabled = true;
            
            followPath.enabled = true;
	            //Debug.DrawLine(rb.transform.position, target, Color.red);

	            followPath.PathEndReachedEvent += FollowPathOnPathEndReachedEvent;

	            //Exit early if no enemies in sight
	            if (vision.enemyInSight.Count > 0)
	            {
		            followPath.ActivatePathToTarget(target);


		            foreach (Transform neighbour in neighbours.neighbours)
		            {
			            neighbour.GetComponentInParent<ControllerSwarmer>().canAttack = true;
			            neighbour.GetComponentInParent<ControllerSwarmer>().canSwarm = false;
			            neighbour.GetComponentInParent<ControllerSwarmer>().target = vision.enemyInSight[0];
			            neighbour.GetComponentInParent<ControllerSwarmer>().myOwnerAlienAI = controller;
			            //new WaitForSeconds(2f);
			            //neighbour.transform.position = new Vector3(vision.enemyInSight[0].transform.position.x + Random.Range(-5, 5), vision.enemyInSight[0].transform.position.y, vision.enemyInSight[0].transform.position.z + Random.Range(-5, 5));
		            }
	            }
            
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
	        base.Execute(aDeltaTime, aTimeScale);

	        if (vision.enemyInSight.Count > 0)
	        {
		        if (vision.enemyInSight[0] != null)
		        {
			        turnTowards.targetPosition = target;

			        foreach (Transform neighbour in neighbours.neighbours)
			        {
				        neighbour.GetComponentInParent<ControllerSwarmer>().canAttack = true;
				        neighbour.GetComponentInParent<ControllerSwarmer>().canSwarm = false;
				        neighbour.GetComponentInParent<ControllerSwarmer>().target = vision.enemyInSight[0];
				        neighbour.GetComponentInParent<ControllerSwarmer>().myOwnerAlienAI = controller;
				        //new WaitForSeconds(2f);
				        //neighbour.transform.position = new Vector3(vision.enemyInSight[0].transform.position.x + Random.Range(-5, 5), vision.enemyInSight[0].transform.position.y, vision.enemyInSight[0].transform.position.z + Random.Range(-5, 5));
			        }

			        if (vision.enemyInSight.Count == 0)
				        Finish();
		        }
	        }
	        else if (vision.enemyInSight.Count == 0)
		        Finish();
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
            controller.rend.material.shader = controller.defaultShader;
            testShapes.colour = Color.green;
            testShapes.intensity = 1;
            followPath.PathEndReachedEvent -= FollowPathOnPathEndReachedEvent;
            
            /*
            foreach (Transform neighbour in neighbours.neighbours)
            {
                neighbour.GetComponentInParent<ControllerSwarmer>().canAttack = false;
                neighbour.GetComponentInParent<ControllerSwarmer>().canSwarm = true;
            }
            */
        }
    }
}
