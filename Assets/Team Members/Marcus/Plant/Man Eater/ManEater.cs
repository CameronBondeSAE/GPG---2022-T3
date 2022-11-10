using System;
using System.Collections;
using System.Collections.Generic;
using Marcus;
using UnityEngine;

public class ManEater : MonoBehaviour, IFlammable
{
    public ColissionManager colissionManager;
    
    private void OnEnable()
    {
        colissionManager.OnTriggerEnterEvent += EatThing;
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
        if (other.GetComponent<MarcusInput>() != null)
        {
            print("Get Nommed");
        }
    }

    #region Interface Functions

    public void SetOnFire()
    {
        //Well die but more dramatically
        //Maybe spread some man eater virus to nearby plants
    }

    public void ChangeHeat(IHeatSource heatSource, float x)
    {
        
    }

    #endregion
}
