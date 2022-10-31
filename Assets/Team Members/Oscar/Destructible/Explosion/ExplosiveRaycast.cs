using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveRaycast : MonoBehaviour
{
    public void ExplosionRaycast()
    {
        //create overlap sphere to do a raycast to set other objects on fire.
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

        foreach (Collider item in colliders) 
        {
            if (item.GetComponent<IFlammable>() != null)
            {
                item.GetComponent<IFlammable>().SetOnFire();
            }
            if (item.GetComponent<Marcus.Health>() != null)
            {
                //take the damage my famalamb
            }
        }
    }
    
}
