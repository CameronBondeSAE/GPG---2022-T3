using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveFragments : MonoBehaviour
{
    public ExplosiveRaycast explosiveRaycast;

    IEnumerator Start()
    {
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        explosiveRaycast.ExplosionRaycast();
        
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
