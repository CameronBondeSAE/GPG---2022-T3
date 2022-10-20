using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    private Renderer rend;

    private void Start()
    {
        rend = this.GetComponent<Renderer>();
        rend.material.SetColor("_BaseColor", Color.red);
    }

    private void OnTriggerEnter(Collider x)
    {
        if (x.GetComponent<IFlammable>() != null)
        {
            x.GetComponent<IFlammable>().SetOnFire();
        }
    }
    
}
