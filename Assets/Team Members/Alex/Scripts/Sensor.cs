using System;
using Anthill.AI;
using UnityEngine;

namespace Alex
{
    public class Sensor : MonoBehaviour, ISense
    {
        public Controller controller;

        //Update all world conditions in this code
        //Health, sound, vision, etc
        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            aWorldState.Set(Scenario.seeResource, controller.CanSeeResource());
            aWorldState.Set(Scenario.atResource, controller.AtResource());
            aWorldState.Set(Scenario.hasResource, controller.HasResource());
            aWorldState.Set(Scenario.atBase, controller.AtDropOffPoint());
            aWorldState.Set(Scenario.resourceCollected, controller.ResourceCollected());
            aWorldState.Set(Scenario.seeEnemy, controller.CanSeeEnemy());
            aWorldState.Set(Scenario.hearEnemy, controller.CanSeeEnemy());
            //aWorldState.Set(Scenario.canAttack, controller.CanAttack());
            //aWorldState.Set(Scenario.isAttacking, controller.IsAttacking());
            //aWorldState.Set(Scenario.enemyDead, controller.EnemyDead());
            
            //aWorldState.Set(Scenario.wondering, controller.Wondering());
            //aWorldState.Set(Scenario.followingPath, controller.FollowingPath());
        }
        
        public enum Scenario
        {
            seeResource = 0,
            atResource = 1,
            hasResource = 2,
            atBase = 3,
            seeEnemy = 4,
            resourceCollected = 5,
            hearEnemy = 6
        }
    }
}