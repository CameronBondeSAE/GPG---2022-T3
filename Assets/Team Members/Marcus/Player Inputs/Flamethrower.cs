using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public MarcusInput player;

    public BoxCollider idealZone;
    public BoxCollider falloffZone;
    
    // Start is called before the first frame update
    void Start()
    {
        player.FireEvent += ActivateTriggers;
        player.StopEvent += DisableTriggers;

        idealZone.enabled = false;
        falloffZone.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void ActivateTriggers()
    {
        idealZone.enabled = true;
        falloffZone.enabled = true;
    }

    private void DisableTriggers()
    {
        idealZone.enabled = false;
        falloffZone.enabled = false;
    }
}
