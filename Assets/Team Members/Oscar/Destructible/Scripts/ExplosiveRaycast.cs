using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveRaycast : MonoBehaviour, IHeatSource
{
    public float explodeRadius = 5f;
    public float heat = 500;
    public float explodeForce = 500;
    private IHeatSource theHeatSource;
    
    public void ExplosionRaycast()
    {
        //create overlap sphere to do a raycast to set other objects on fire.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider item in colliders)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                SendFlying(rb);
            }
            
            if (item.GetComponent<IFlammable>() != null)
            {
                item.GetComponent<IFlammable>().ChangeHeat(theHeatSource, heat);
            }
            if (item.GetComponent<Marcus.Health>() != null)
            {
                //take the damage my famalamb
            }
        }
    }

    private void SendFlying(Rigidbody rb)
    {
        rb.AddExplosionForce(explodeForce, transform.position, explodeRadius);
    }
}
