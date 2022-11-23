using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

namespace Alex
{
    public class SwarmerAttackingState : AntAIStatesSwarmer
    {
        public GameObject owner;

        [SerializeField] Align align;
        [SerializeField] Separation separation;
        [SerializeField] Cohesion cohesion;
        Neighbours neighbours;
        ControllerSwarmer controllerSwarmer;
        TurnTowards turnTowards;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            
            owner = aGameObject;
            align = aGameObject.GetComponent<Align>();
            separation = aGameObject.GetComponent<Separation>();
            cohesion = aGameObject.GetComponent<Cohesion>();
            neighbours = aGameObject.GetComponent<Neighbours>();
            controllerSwarmer = aGameObject.GetComponent<ControllerSwarmer>();
            turnTowards = aGameObject.GetComponent<TurnTowards>();
        }

        public override void Enter()
        {
            base.Enter();

            align.enabled = false;
            separation.enabled = false;
            cohesion.enabled = false;
            
            Debug.Log("AttackState entered");

            Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);

            Debug.Log("AttackState entered");


            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}
