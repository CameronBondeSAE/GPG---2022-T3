using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddExplosionForce(5f,new Vector3(Random.Range(0f,5f), 0, Random.Range(0f,5f)), 3f);
    }
}
