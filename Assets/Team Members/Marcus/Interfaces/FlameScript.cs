using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameScript : MonoBehaviour
{
    public PHealth thing;
    
    private float fireDamage = 5f;
    
    private void OnTriggerEnter(Collider collisionData)
    {
        if (collisionData.GetComponent<PHealth>())
        {
            thing = collisionData.GetComponent<PHealth>();
            thing.Damaged(fireDamage);
        }

        if (collisionData.GetComponent<IFlammable>() != null)
        {
           // collisionData.GetComponent<IFlammable>().SetOnFire();
        }
    }
}

