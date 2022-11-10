using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveFragments : MonoBehaviour
{
    
    IEnumerator Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddExplosionForce(5f,Vector3.forward, 5,  3f);
        
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
