using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

namespace Alex
{
    public class WanderingState : AntAIState
    {

        public Wander wander;

        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;
        TurnTowards turnTowards;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            owner = aGameObject;
            wander = aGameObject.GetComponent<Wander>();
            turnTowards = aGameObject.GetComponent<TurnTowards>();
            
        }

        public override void Enter()
        {
            base.Enter();
            wander.enabled = true;
            wander.turnForce = 700;
            turnTowards.enabled = false;
            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            wander.turnForce = 20;
            Finish();
        }
    }
}
