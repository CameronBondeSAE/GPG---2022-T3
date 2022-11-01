using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    // I needa rejig this whole thing
    // rather than using triggers I need to instantiate a fireball
    // the fireball has to have no air resistance and high ground friction
    // once it hits an object it attaches to it (or creates a clone on it)
    // and then it tells the other thing it's now on fire
    /*public GameObject fireball;
    private Vector3 flameSpeed;

    void CreateFlame()
    {
        gameObject flame = Instantiate(fireball, transform.forward, Quaternion.identity);
        flame.GetComponent<Rigidbody>().AddForce(flameSpeed)
    }*/
    
    
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
        print("Triggered on: " + other.gameObject.name);
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
