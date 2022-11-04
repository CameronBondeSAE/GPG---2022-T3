using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManEater : MonoBehaviour, IFlammable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.OverlapSphere(transform.position, 2))
    }

    public void SetOnFire()
    {
        //Well die but more dramatically
        //Maybe spread some man eater virus to nearby plants
    }
}
