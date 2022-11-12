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
    private float radius;
    private Vector3 center;
    [SerializeField]private float minDistance;
    private float distance;
    [SerializeField] private float proximityMultiplier;
    
    private Vector3 burnVictim;
    
    //Setters
    //fire stats are set by HeatComponent
    //
    public void SetFlameStats(float x, float y, float z)
    {
        heat = x;
        fuel = y;
        radius = z;
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
        
        //change this
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
        fuel--;
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
