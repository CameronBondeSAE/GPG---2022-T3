using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Swoop : MonoBehaviour
{
    public Collider[] NearbyObjects;
    public float radius;
    private float y;
    private Dive dScript;
    
    public GameObject target;
    public float speed;
    public float distStop = 3;
    private bool detc;
    // Start is called before the first frame update
    void Start()
    {
        dScript = GetComponent<Dive>();
        y = transform.position.y;
        detc = false;
    }

    // Update is called once per frame
    void Update()
    {
        //NearbyObjects = Physics.OverlapCapsule(transform.position, transform.position + Vector3.up * 10, radius);

        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();
        direction = direction * Time.deltaTime * speed;
        
        Vector3 targetZ = new Vector3(target.transform.position.x, y, target.transform.position.z);
        float dist = Vector3.Distance(targetZ, transform.position);

        if (dist > distStop && !detc) 
        {
            direction.y = 0;
        }
        else
        {
            dScript.enabled = true;
            this.enabled = false;
        }

        transform.Translate(direction);
    }

    private void OnDrawGizmos()
    {
        
    }
    
}
