using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GooPuddle : MonoBehaviour
{
    void Update()
    {
        //create overlap sphere to do a raycast to set other objects on fire.
        Collider[] colliders = Physics.OverlapBox(new Vector3(0,transform.position.y, 0), 
            new Vector3(transform.localScale.x/2,0.5f,
                transform.localScale.z/2));

        foreach (Collider item in colliders) 
        {
            //if the thing that steps on the puddle, reduce stats or increase drag maybe
            if (item.GetComponent<OscarPlayerStats>() != null)
            {
                //increase drag or reduce speed
                item.GetComponent<Rigidbody>().drag = 1f;
            }
        }
    }
}
