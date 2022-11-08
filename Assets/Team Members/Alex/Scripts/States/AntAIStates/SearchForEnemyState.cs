using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{

    public class SearchForEnemyState : AntAIState
    {
        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;
        [SerializeField]
        Vision vision;
        Rigidbody rb;
        Controller controller;
        private TurnTowards turnTowards;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            owner = aGameObject;
            controller = aGameObject.GetComponent<Controller>();
            vision = aGameObject.GetComponent<Vision>();
            rb = aGameObject.GetComponent<Rigidbody>();
            turnTowards = aGameObject.GetComponent<TurnTowards>();
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
                turnTowards.targetPosition = vision.enemyInSight[0].transform.position;
                Finish();
            }
        }

        public override void Exit()
        {
            
        }
    }
}