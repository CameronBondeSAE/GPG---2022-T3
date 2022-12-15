using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{

    public float soundRadius;
    
    
    void EmitSound()
    {
        Collider[] overlapSphere = Physics.OverlapSphere(transform.position, soundRadius);

        foreach (Collider collider1 in overlapSphere)
        {
            if (GetComponent<HearingComponent>())
            {
                
            }
        }
    }
    
}
