using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class BarrelHealth : MonoBehaviour
{
    public Barrel barrel;
    float amount;

    private float damage = 0.1f;
    
    public void OnEnable()
    {
        barrel.burning += Damage;
    }

    public float myAmount(float myHealth)
    {
        return amount;
    }
    
    public void Damage()
    {
        amount-=damage;
        print("ow!" + amount + " health left");
    }
}
