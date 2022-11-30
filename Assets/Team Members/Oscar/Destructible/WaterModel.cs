using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterModel : MonoBehaviour
{
    public float radius;

    private Flammable flammable;
    
    private IHeatSource theHeatSource;
    
    void FixedUpdate()
    {
        Collider[] splashColliders = Physics.OverlapSphere(transform.position, radius, 9999999, QueryTriggerInteraction.Collide);
        foreach (Collider item in splashColliders)
        {
            if (item.GetComponent<Flammable>() != null)
            {
                flammable = item.GetComponent<Flammable>();
                flammable.ChangeHeat(theHeatSource,-20);
                flammable.Extinguish();
                flammable.OnCoolDown();
                
            }
        }
    }
}
