using System.Collections;
using System.Collections.Generic;
using Alex;
using Anthill.AI;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace Alex
{



    public class Sensor : MonoBehaviour, ISense
    {
        public List<Transform> visionTargets;
        public List<Transform> resourceTargets;

        //Update all world conditions in this code
        //Health, sound, vision, etc
        public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
        {
            aWorldState.Set(Scenario.seeResource, CanSeeResource());
            aWorldState.Set(Scenario.atResource, AtResource());
            aWorldState.Set(Scenario.hasResource, HasResource());
            aWorldState.Set(Scenario.atBase, AtBase());
            aWorldState.Set(Scenario.resourceCollected, ResourceCollected());

            aWorldState.Set(Scenario.seeEnemy, CanSeeEnemy());
            aWorldState.Set(Scenario.canAttack, CanAttack());
            aWorldState.Set(Scenario.isAttacking, IsAttacking());
            aWorldState.Set(Scenario.enemyDead, EnemyDead());
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
            isAttacking = 8
        }

        private bool CanSeeEnemy()
        {
            foreach (Transform visionTarget in visionTargets)
            {
                if (Physics.Linecast(transform.position, visionTarget.transform.position, 255,
                        QueryTriggerInteraction.Ignore))
                {
                    return true;
                }
            }
            return false;
        }


        private bool CanAttack()
        {
            return false;
        }
        
        private bool IsAttacking()
        {
            return false;
        }
        
        private bool EnemyDead()
        {
            return false;
        }
        
        
        private bool CanSeeResource()
        {
            foreach (Transform resourceTarget in resourceTargets)
            {
                if (Physics.Linecast(transform.position, resourceTarget.transform.position, 255,
                        QueryTriggerInteraction.Ignore))
                {
                    return true;
                }
            }
            return false;
        }

        private bool AtResource()
        {
            return false;
        }
        
        private bool HasResource()
        {
            return false;
        }
            
        private bool AtBase()
        {
            return false;
        }
        
        private bool ResourceCollected()
        {
            return false;
        }
    }
}