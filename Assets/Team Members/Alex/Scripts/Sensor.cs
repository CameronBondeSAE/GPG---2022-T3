using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alex;
using Anthill.AI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

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
            aWorldState.Set(Scenario.canAttack, controller.CanAttack());
            aWorldState.Set(Scenario.isAttacking, controller.IsAttacking());
            aWorldState.Set(Scenario.enemyDead, controller.EnemyDead());
            
            aWorldState.Set(Scenario.wondering, controller.Wondering());
            aWorldState.Set(Scenario.followingPath, controller.FollowingPath());
        }
        
        public enum Scenario
        {
            seeResource = 0,
            atResource = 1,
            hasResource = 2,
            atBase = 3,
            resourceCollected = 4,
            enemyDead = 5,
            canAttack = 6,
            seeEnemy = 7,
            isAttacking = 8,
            wondering = 9,
            followingPath = 10
            
        }
    }
}