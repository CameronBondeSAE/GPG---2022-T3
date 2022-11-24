using System.Collections;
using System.Collections.Generic;
using Alex;
using Lloyd;
using UnityEngine;

namespace Alex
{
    public class SwarmerAttackingState : AntAIStatesSwarmer
    {
        public GameObject owner;

        [SerializeField] Align align;
        [SerializeField] Separation separation;
        [SerializeField] Cohesion cohesion;
        [SerializeField] AttackSphereAndShader attackSphereAndShader;
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
            attackSphereAndShader = aGameObject.GetComponent<AttackSphereAndShader>();
        }

        public override void Enter()
        {
            base.Enter();

            align.enabled = false;
            separation.enabled = false;
            cohesion.enabled = false;
            attackSphereAndShader.enabled = true;
            attackSphereAndShader.attackCollider.SetActive(true);
            turnTowards.enabled = true;

            turnTowards.targetTransform = controllerSwarmer.target;

            
            Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            //attackSphereAndShader.gameObject.SetActive(false);
            attackSphereAndShader.enabled = false;
            attackSphereAndShader.attackCollider.SetActive(false);
            turnTowards.enabled = false;
            Finish();
        }
    }
}
