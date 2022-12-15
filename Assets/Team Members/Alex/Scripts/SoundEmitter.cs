using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{

    public float soundRadius;
    
    [Button]
    void EmitSound()
    {
        Collider[] overlapSphere = Physics.OverlapSphere(transform.position, soundRadius);

        foreach (Collider itemInList in overlapSphere)
        {
            if (itemInList.GetComponent<HearingComponent>())
            {
                itemInList.GetComponent<HearingComponent>().HeardSomething(this, soundRadius);
            }
        }
    }
}
