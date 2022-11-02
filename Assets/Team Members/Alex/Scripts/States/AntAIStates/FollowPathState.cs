using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

/*
namespace Alex
{

    public class FollowPathState : AntAIState
    {
        public GameObject owner;
        [SerializeField]
        FollowPath followPath;
        Rigidbody rb;
        Controller controller;
        Vision vision;
        TurnTowards turnTowards;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            owner = aGameObject;
            followPath = aGameObject.GetComponent<FollowPath>();
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

            if (vision.enemyInSight.Count > 0)
                followPath.enabled = true;

            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            followPath.enabled = false;
            Finish();
        }
    }
}
*/