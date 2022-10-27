using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameScript : MonoBehaviour
{
    public Heath_UnivComp thing;
    
    private float fireDamage = 5f;
    
    private void OnTriggerEnter(Collider collisionData)
    {
        if (collisionData.GetComponent<Heath_UnivComp>())
        {
            thing = collisionData.GetComponent<Heath_UnivComp>();
            thing.Damaged(fireDamage);
        }

        if (collisionData.GetComponent<IFlammable>() != null)
        {
            collisionData.GetComponent<IFlammable>().SetOnFire();
        }
    }
}

