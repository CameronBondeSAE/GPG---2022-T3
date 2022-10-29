using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SludgeExplodeState : MonoBehaviour
{
    public GameObject sludgeGoo;
    void Start()
    {
        //spawn sludge
        GameObject goo = Instantiate(sludgeGoo, transform.position + new Vector3(0,-1,0), quaternion.identity);
        goo.transform.localScale = new Vector3(Random.Range(1f,4f),0.05f,Random.Range(1f,4f));
        
        //goo has been spawned so just destroy the remains of the canister
        DestroySludgeModel();
    }
    void DestroySludgeModel()
    {
        Destroy(gameObject);
    }
}
