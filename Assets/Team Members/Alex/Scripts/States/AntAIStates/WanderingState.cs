using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

namespace Alex
{
    public class WanderingState : AntAIState
    {

        public Wonder wonder;

        // Reference to my main GameObject, so I can access all the normal code I have in there.
        public GameObject owner;

        public override void Create(GameObject aGameObject)
        {
            base.Create(aGameObject);
            wonder = aGameObject.GetComponent<Wonder>();

            owner = aGameObject;
        }

        public override void Enter()
        {
            base.Enter();
            wonder.enabled = true;
            wonder.turnForce = 700;
            Finish();
        }

        public override void Exit()
        {
            base.Exit();
            wonder.turnForce = 20;
            Finish();
        }
    }
}
