using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

namespace Alex
{
    public class SwarmingState : AntAIStatesSwarmer
    {
        public GameObject owner;
        [SerializeField] Align align;
        [SerializeField] Separation separation;
        [SerializeField] Cohesion cohesion;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);

            owner = aGameObject;
            align = aGameObject.GetComponent<Align>();
            separation = aGameObject.GetComponent<Separation>();
            cohesion = aGameObject.GetComponent<Cohesion>();
        }

        public override void Enter()
        {
            base.Enter();
            align.enabled = true;
            separation.enabled = true;
            cohesion.enabled = true;
            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            Finish();
        }
    }
}