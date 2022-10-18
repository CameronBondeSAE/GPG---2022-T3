using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alex;
using Anthill.AI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class Controller : MonoBehaviour
{
    public List<Target> enemyTargets;
    public List<Resource> resourceTargets;
    public List<DropOffPoint> dropOffPoints;
    public bool hasResource;
    public bool canAttack;
    public bool isAttacking;
    public bool enemyDead;
    [SerializeField]
    Vision vision;

    public Rigidbody rb;
    public Collider collider;
    public Inventory inventory;
    public int turnSpeed;

    public void Awake()
    {
        hasResource = false;
        resourceTargets = FindObjectsOfType<Resource>().ToList();
        enemyTargets = FindObjectsOfType<Target>().ToList();
        dropOffPoints = FindObjectsOfType<DropOffPoint>().ToList();
    }

    public void Update()
    {
        resourceTargets = FindObjectsOfType<Resource>().ToList();
    }

    public bool CanSeeEnemy()
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


    public bool CanAttack()
    {
        return false;
    }

    public bool IsAttacking()
    {
        return false;
    }

    public bool EnemyDead()
    {
        return false;
    }


    public bool CanSeeResource()
    {
        return vision.resourcesInSight.Count > 0;
    }


    public bool AtResource()
    {
        //return false;
        return Vector3.Distance(resourceTargets[0].transform.position, rb.transform.position) < 0.2f;
        
    }

    public bool HasResource()
    {
        return inventory.capacityReached;
    }

    public bool AtBase()
    {
        return false;
    }

    public bool ResourceCollected()
    {
        return false;
    }
}

