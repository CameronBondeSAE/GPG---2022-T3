using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour, IFlammable
{
    public PHealth health;
    private float myHealth = 100f;

    // Start is called before the first frame update
    void Start()
    {
        health.damagedEvent += FlowerHurty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FlowerHurty(float damage)
    {
        myHealth -= damage;
        print("Ouchies");
    }

    public void SetOnFire()
    {
        // Catch Fire
    }
}
