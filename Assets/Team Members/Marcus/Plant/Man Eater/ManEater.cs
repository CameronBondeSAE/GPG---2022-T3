using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManEater : MonoBehaviour, IFlammable
{
    public Collider[] targets;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targets = Physics.OverlapSphere(transform.position, 2);
        foreach (Collider item in targets)
        {
            if (item.GetComponent<MarcusInput>())
            {
                print("Get nommed");
            }
        }
    }
    
    public void SetOnFire()
    {
        //Well die but more dramatically
        //Maybe spread some man eater virus to nearby plants
    }
}
