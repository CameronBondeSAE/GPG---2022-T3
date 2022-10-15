using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alex;
using Anthill.AI;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace Alex
{
    public class Sensor : MonoBehaviour, ISense
    {
        public List<Target> enemyTargets;
        public List<Resource> resourceTargets;
        public bool hasResource;
        public bool canAttack;
        public bool isAttacking;
        public bool enemyDead;
        [SerializeField]
        Vision vision;

        public Rigidbody rb;
        public Collider collider;
        public float turnSpeed = 1;

        private void Awake()
        {
            hasResource = false; 
        }

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

        void Start()
        {
            resourceTargets = FindObjectsOfType<Resource>().ToList();
            enemyTargets = FindObjectsOfType<Target>().ToList();
        }

        private bool CanSeeEnemy()
        {
            foreach (Target visionTarget in enemyTargets)
            {
                RaycastHit RayHitInfo;
                
                if (Physics.Linecast(transform.position, visionTarget.transform.position, out RayHitInfo, 255,
                        QueryTriggerInteraction.Ignore))
                    
                {
                    if (RayHitInfo.transform == visionTarget)
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
            return vision.resourcesInSight.Count > 0;
        }


        private bool AtResource()
        {
            return Vector3.Distance(vision.resourcesInSight[0].transform.position,  rb.transform.position) < 0.2f;
        }
        
        private bool HasResource()
        {
            return hasResource = true;
        }
            
        private bool AtBase()
        {
            return false;
        }
        
        private bool ResourceCollected()
        {
            return false;
        }

        public void MoveToResource()
        {
            if (vision.resourcesInSight.Count == 0) return;


            Vector3 forwards = transform.forward;
            Vector3 towardResource = vision.resourcesInSight[0].position - rb.transform.position;
            float angle = Vector3.SignedAngle(forwards, towardResource, Vector3.up);
            if (vision.resourcesInSight == null) ;
            rb.AddTorque(new Vector3(0, angle * turnSpeed, 0));
        }

        public void GatherResource()
        {
            Vector3 myPosition = transform.position;
            Vector3 targetPosition = vision.resourcesInSight[0].position;


            if (Vector3.Distance(myPosition, targetPosition) < 0.2f)
                hasResource = true;
            vision.resourcesInSight.Clear();
        }

        public void DropOffResource()
        {
            
        }
    }
}