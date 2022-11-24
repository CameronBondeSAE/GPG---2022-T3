using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscarExplosiveForce : MonoBehaviour
{
    public float radius;
    
    public float forceForce;

    private void OnEnable()
    {
        explosiveforcestuff();
    }

    void explosiveforcestuff()
    {
        Vector3 explosivePOS = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosivePOS, radius);
        foreach (Collider item in colliders)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            
            if (item.GetComponent<Rigidbody>() != null)
            {
                rb.AddExplosionForce(forceForce, transform.position,radius);
            }
        }
    }
}
