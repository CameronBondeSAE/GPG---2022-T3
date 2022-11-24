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
            aWorldState.Set(Scenario.Attack, controllerSwarmer.IsAttacking());
            aWorldState.Set(Scenario.Wandering, controllerSwarmer.IsSwarming());
        }

        public enum Scenario
        {
	        Attack = 0,
	        Wandering = 1,
	        HurtEnemy = 2
        }
    }
}
