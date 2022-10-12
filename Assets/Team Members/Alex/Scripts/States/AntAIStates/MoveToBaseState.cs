using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Alex
{
    public class MoveToBaseState : AntAIState
    {
        public override void Enter()
        {
            base.Enter();

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
            Finish();
        }
    }
}