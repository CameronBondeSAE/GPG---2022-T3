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
    private float fuel;
    
    //fire does more damage depending on proximity
    [SerializeField] private float radius;
    [SerializeField] private Vector3 center;
    [SerializeField] private float minDistance;
    [SerializeField] private float distance;
    [SerializeField] private float proximityMultiplier=2;
    
    private Vector3 burnVictim;
    
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
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<IFlammable>() != null)
            {
                IFlammable[] flammables = hitCollider.GetComponents<IFlammable>();
                foreach (IFlammable item in flammables)
                {
                    distance = Vector3.Distance(center, burnVictim);
                    if (distance > minDistance)
                    {
                        hitCollider.GetComponent<IFlammable>().ChangeHeat(myself, heat * proximityMultiplier);
                    }
                    
                    item.ChangeHeat(myself, heat);
                }
            }
        }
    }

    public void ChangeFuel(float amount)
    {
        fuel += amount;
    }

    private void TickTock()
    {
        fuel -= 0.2f;
        if (fuel <= 0)
        {
            FlameOut();
        }
    }

    private void FlameOut()
    {
        Destroy(this);
    }
}
