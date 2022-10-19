using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class AttackingEnemyState : AntAIState
    {
        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;
        [SerializeField]
        Vision vision;
        Rigidbody rb;
        Controller controller;

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
            
            if (vision.enemyInSight.Count == 0 && vision.enemyInSight != null) return;
            if (vision.enemyInSight.Count > 0)
            {
                owner.GetComponent<TurnTowards>().targetPosition = vision.enemyInSight[0].transform.position;
                //owner.GetComponent<TurnTowards>().targetTransform = vision.resourcesInSight[0].transform.position;
                Finish();
            }
        }
    }
}