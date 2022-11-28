using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlameModel : MonoBehaviour, IHeatSource
{   
    //how much damage flame does
    private float heat;
 
     //how much fuel it has to burn
    [SerializeField] private float fuel;
    
    //fire does more damage depending on proximity
    [SerializeField] private float radius;
    [SerializeField] private Vector3 center;
    [SerializeField] private float minDistance;
    private float distance;
    [SerializeField] private float proximityMultiplier;
    
    private Vector3 burnVictim;

    private Flammable flammable;
    
    //Setters
    //fire stats are set by HeatComponent
    //
    public void SetFlameStats(float x, float y, float z)
    {
        heat = x;
        fuel = y;
        radius = z;

        minDistance = radius / 2;

        transform.localScale = new Vector3(radius, radius, radius);
    }

    private IHeatSource myself;

    private void OnEnable()
    {
        myself = GetComponent<IHeatSource>();
        center = transform.position;
    }

    private void FixedUpdate()
    {
        CastFire();

        TickTock();
    }

    private void CastFire()
    {
        //change this thru HeatComponent when spawning
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, 9999999, QueryTriggerInteraction.Collide);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Flammable>() != null)
            {
                Flammable[] flammables = hitCollider.GetComponents<Flammable>();
                foreach (Flammable item in flammables)
                {
                    flammable = item.GetComponent<Flammable>();
                    distance = Vector3.Distance(center, burnVictim);
                    if (distance > minDistance)
                    {
                        flammable.ChangeHeat(myself, heat * proximityMultiplier);
                    }
                    flammable.ChangeHeat(myself, heat);
                }
            }
        }
    }

    private void TickTock()
    {
        if (fuel > 0 && radius > 0)
        {
            fuel -= 0.2f;
            radius -= 0.2f;
        }
        else if (fuel <= 0)
        {
            FlameOut();
        }
    }

    private void FlameOut()
    {
        Destroy(this);
    }
}
