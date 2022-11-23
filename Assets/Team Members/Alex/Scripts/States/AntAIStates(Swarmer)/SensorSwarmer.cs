using System.Collections;
using System.Collections.Generic;
using Anthill.AI;
using UnityEngine;

namespace Alex
{
    public class SensorSwarmer : MonoBehaviour, ISense
    {

        public ControllerSwarmer controllerSwarmer;

        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            aWorldState.Set(Scenario.isAttacking, controllerSwarmer.IsAttacking());
            aWorldState.Set(Scenario.isSwarming, controllerSwarmer.IsSwarming());
        }

        public enum Scenario
        {
            isAttacking = 1,
            isSwarming = 2
        }
    }
}
