using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlameModel : MonoBehaviour, IHeatSource
{   
    //how much damage flame does
    private float heat;
 
     //how much fuel it has to burn
    [SerializeField] private float fuel;

    [SerializeField] private float deteriorateRate;
    
    //fire does more damage depending on proximity
    [SerializeField] private float radius;
    [SerializeField] private Vector3 center;
    [SerializeField] private float minDistance;
    private float distance;
    [SerializeField] private float proximityMultiplier;
    
    private Vector3 burnVictim;

    private Flammable flammable;

    [SerializeField]private int maxRoundRobin;
    private float roundRobin;

    private float randomRobin;
    
    private NetworkManager _nm;
    
    private void OnEnable()
    {
	    _nm = NetworkManager.Singleton;
	    if (!_nm.IsServer) return;
        randomRobin = Random.Range(0, 0.1f);
        roundRobin += randomRobin;
    }

    //Setters
    //fire stats are set by HeatComponent
    //
    public void SetFlameStats(float _heat, float _fuel, float _radius)
    {
        heat = _heat;
        fuel = _fuel;
        radius = _radius;

        minDistance = radius / 2;

        transform.localScale = new Vector3(radius, radius, radius);
    }

    private void FixedUpdate()
    {
	    if (!_nm.IsServer) return;
	    roundRobin++;
        if (roundRobin <= maxRoundRobin)
        {
            CastFire();

            TickTock();
            roundRobin = 0;
        }
    }

    private void CastFire()
    {
        center = transform.position;

        //would it be more efficient to run two overlap spheres or calculate dist with one sphere?
        //
        
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, 9999999, QueryTriggerInteraction.Collide);
        foreach (var hitCollider in hitColliders)
        {
            //GameObject fire = Instantiate(_fire01Prefab, transform.position, Quaternion.identity) as GameObject;

            if (hitCollider.GetComponent<Flammable>() != null)
            {
                hitCollider.GetComponent<Flammable>().ChangeHeat(this, heat);

                Vector3 burnVictim = hitCollider.transform.position;

                distance = Vector3.Distance(center, burnVictim);
                if (distance > minDistance)
                {
                    hitCollider.GetComponent<Flammable>().ChangeHeat(this, heat * proximityMultiplier);
                }
                //transform.SetParent(hitCollider.transform);
            }
        }
    }

    private void TickTock()
    {
        if (fuel > 0 && radius > 0)
        {
            fuel -= deteriorateRate * Time.deltaTime;
            radius -= deteriorateRate * Time.deltaTime;
        }

        if (radius <= 0)
            radius = 0;

        else if (fuel <= 0)
        {
            fuel = 0;
            FlameOut();
        }
    }

    public void FlameOut()
    {
	    Destroy(this);
    }
}
