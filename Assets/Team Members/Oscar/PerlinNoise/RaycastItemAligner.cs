using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RaycastItemAligner : MonoBehaviour
{
    public float raycastDistance = 100f;
    public GameObject objectToSpawn;
    public float overlapTestBoxSize = 1f;
    public LayerMask spawnedObjectLayer;

    private void Start()
    {
        PositionRaycast();
    }

    public void PositionRaycast()
    {
        RaycastHit hit;
        
        //if the raycast    random position 10 units above    cast down              public float
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            //figures out the rotation when its casted.
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[1];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapTestBoxScale,
                collidersInsideOverlapBox, spawnRotation, spawnedObjectLayer);

            if (numberOfCollidersFound == 0)
            {
                Pick(hit.point,spawnRotation);
            }
        }
    }

    void Pick(Vector3 positionToSpawn, Quaternion rotationToSpawn)
    {
        GameObject Items = Instantiate(objectToSpawn, positionToSpawn, rotationToSpawn);
    }
}
