using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlameModel : MonoBehaviour, IHeatSource
{
    [SerializeField] private float fuel;

    private Vector3 center;

    private float radius;

    public void SetRadius(float amount)
    {
        radius = amount;
    }

    private float fireDamage;

    public void SetFireDamage(float amount)
    {
        fireDamage = amount;
    }

    private IHeatSource myself;

    private void OnEnable()
    {
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
                    item.ChangeHeat(myself, fireDamage);
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
            Destroy(myself);
        }
    }
}
