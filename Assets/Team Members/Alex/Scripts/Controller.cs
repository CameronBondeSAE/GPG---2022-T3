using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Alex;
using Anthill.AI;
using Lloyd;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

namespace Alex
{
    public class Controller : MonoBehaviour

    {


    public bool hasResource;
    public bool canAttack;
    public bool isAttacking;
    public bool enemyDead;
    [SerializeField] public Vision vision;

    public Rigidbody rb;
    public Inventory inventory;
    public int turnSpeed;
    public bool followingPath;
    public AStar aStar;
    public Renderer rend;
    public Shader defaultShader;
    public Shader lowHeatShader;
    public TurnTowards turnTowards;
    public Flammable flammable;
    Color lerpedColor1 = Color.red;

    public Transform myBase;

    public void Awake()
    {
        hasResource = false;
        vision = FindObjectOfType<Vision>();
        turnTowards.enabled = false;
        //interact = FindObjectOfType<Interact>();

        myBase = AlienBase().transform;
        vision.dropOffPointsFound.Add(myBase);
        //FindObjectsOfType<Checkpoint>();
    }
    
    private GameObject AlienBase()
    {
        GameObject spawnPointObject = null;
        foreach (HQ hq in FindObjectsOfType<HQ>())
        {
            if (hq.type == HQ.HQType.Aliens)
            {
                spawnPointObject = hq.GetComponentInChildren<Checkpoint>().gameObject;
            }
        }
        return spawnPointObject;
    }

    /*
    private void FixedUpdate()
    {

        if (flammable.heatLevel > 0 && flammable.heatLevel <= 20)
        {
            lerpedColor1 = Color.Lerp(Color.red, Color.yellow, Mathf.PingPong(Time.time, 1));
            renderer.material.color = lerpedColor1;
        }
    }
    */

    public bool CanSeeEnemy()
    {
        return vision.enemyInSight.Count > 0;
    }

    public bool FollowingPath()
    {
        //if (enemies.Count > 0)
        // return aStar.FindPathStartCoroutine(Vector3Int.FloorToInt(rb.transform.position),Vector3Int.FloorToInt(enemies[0].transform.position));
        // if(resourceTargets.Count > 0)
        //   return(aStar.FindPathStartCoroutine(Vector3Int.FloorToInt(rb.transform.position),Vector3Int.FloorToInt(enemies[0].transform.position)))
        //else
        {
            return false;
        }

    }

/*
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
*/
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
            vision.resourcesInSight.RemoveAll(transformToTest => transformToTest == null);
            return Vector3.Distance(vision.resourcesInSight[0].transform.position, rb.transform.position) < 2f;
        }
        else return false;
    }

    public bool HasResource()
    {
        return GetComponentInChildren<Interact>().storedItems == GetComponentInChildren<Interact>().storedMax;

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

    public bool Wondering()
    {
        return vision.enemyInSight == null && vision.resourcesInSight == null;
    }

    public void ChangeHeat(IHeatSource heatSource, float x)
    {

    }
    
    }
}

