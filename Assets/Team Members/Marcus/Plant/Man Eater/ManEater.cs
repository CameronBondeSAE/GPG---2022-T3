using System;
using System.Collections;
using System.Collections.Generic;
using Marcus;
using UnityEngine;

public class ManEater : MonoBehaviour, IFlammable
{
    public ColissionManager colissionManager;
    public FlammableComponent fireness;
    
    private void OnEnable()
    {
        colissionManager.OnTriggerEnterEvent += EatThing;
        fireness.SetOnFireEvent += SetOnFire;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EatThing(Collider other)
    {
        if (other.GetComponent<IControllable>() != null)
        {
            print("Get Nommed");
        }
    }
    
    public void SetOnFire()
    {
        //Well die but more dramatically
        //Maybe spread some man eater virus to nearby plants
        print("GGGRRR ANGRY DEATH NOISES!!!");
        Destroy(gameObject);
    }

    public void ChangeHeat(IHeatSource heatSource, float x)
    {
        
    }

}
