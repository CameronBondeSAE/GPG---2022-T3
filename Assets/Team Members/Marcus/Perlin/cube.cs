using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    public PerlinThings spawner;
    
    private void OnEnable()
    {
        print("subscribed");
        spawner.UpdateEvent += NewTerrain;
    }

    private void OnDisable()
    {
        print("unsubed");
        spawner.UpdateEvent -= NewTerrain;
    }

    void NewTerrain()
    {
        print("Ded");
        Destroy(this.gameObject);
    }
}
