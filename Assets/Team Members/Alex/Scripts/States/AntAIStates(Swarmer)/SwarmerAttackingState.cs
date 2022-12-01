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
        GameObject vFX;
        
        

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
            attackSphereAndShader = aGameObject.GetComponentInChildren<AttackSphereAndShader>();
            
        }

        public override void Enter()
        {
            base.Enter();

            align.enabled = false;
            separation.enabled = false;
            cohesion.enabled = false;
            turnTowards.enabled = true;

            turnTowards.targetTransform = controllerSwarmer.target;
            
            attackSphereAndShader.vFX.SetActive(true);

            
            Finish();
        }

        public override void Execute(float aDeltaTime, float aTimeScale)
        {
            base.Execute(aDeltaTime, aTimeScale);
            attackSphereAndShader.vFX.SetActive(true);
            
            if(controllerSwarmer.target == null)
                Finish();
        }

        public override void Exit()
        {
            base.Exit();
            turnTowards.enabled = false;
            controllerSwarmer.canAttack = false;
            controllerSwarmer.canSwarm = true;
            attackSphereAndShader.vFX.SetActive(false);
            Finish();
        }
    }
}

