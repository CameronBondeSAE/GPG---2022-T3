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
    public List<Transform> resourceTargets;
    public List<DropOffPoint> dropOffPoints;
    public List<Transform> enemies;
    public bool hasResource;
    public bool canAttack;
    public bool isAttacking;
    public bool enemyDead;
    [SerializeField]
    public Vision vision;

    public Rigidbody rb;
    public Collider collider;
    public Inventory inventory;
    public int turnSpeed;

    public void Awake()
    {
        hasResource = false;
        vision = FindObjectOfType<Vision>();
        enemyTargets = FindObjectsOfType<Target>().ToList();
        dropOffPoints = FindObjectsOfType<DropOffPoint>().ToList();
    }

    public void FixedUpdate()
    {
        enemies = vision.enemyInSight;
        resourceTargets = vision.resourcesInSight;
    }

    public bool CanSeeEnemy()
    {
        return enemies.Count > 0;
    }


    public bool CanAttack()
    {
        if (vision.enemyInSight.Count > 0)
        {
            return Vector3.Distance(vision.enemyInSight[0].transform.position, rb.transform.position) < 0.2f;
        }
        else return false;
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
        if (vision.resourcesInSight.Count > 0)
        {
            return Vector3.Distance(vision.resourcesInSight[0].transform.position, rb.transform.position) < 0.2f;
        }
        else return false;
    }

    public bool HasResource()
    {
        return inventory.capacityReached;
    }

    public bool AtDropOffPoint()
    {
        if (vision.dropOffPointsFound.Count > 0)
        {
            return Vector3.Distance(vision.dropOffPointsFound[0].transform.position, rb.transform.position) < 0.2f;
        }
        else return false;
    }

    public bool ResourceCollected()
    {
        return false;
    }
}

