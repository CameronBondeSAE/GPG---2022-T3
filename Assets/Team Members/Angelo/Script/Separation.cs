using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation : MonoBehaviour
{
    public GameObject child;
    public Transform[] OtherPik;
    public Transform[] TruePik;
    public float SpaceBetween = 1.3f;

    private void OnEnable()
    {
        Gather.UpdateSwarm += UpdateSwarmInfo;
    }

    private void OnDisable()
    {
        Gather.UpdateSwarm -= UpdateSwarmInfo;
    }

    void Start()
    {
        OtherPik = child.transform.GetComponentsInChildren<Transform>();
        TruePik = new Transform[OtherPik.Length - 2];
        int count = 0;
        for (int i = 0; i < OtherPik.Length; i++)
        {
            if (OtherPik.Length > i + 1)
            {
                if (OtherPik[i + 1].position == transform.position)
                {
                    count += 1;
                    i += 1;
                }
            }

            if (OtherPik.Length > i + 1)
            {
                TruePik[i - count] = OtherPik[i + 1];
            }
        }
    }

    
    void Update()
    {
        foreach (Transform AI in TruePik)
        {
            if(AI != gameObject)
            {
                float Distance = Vector3.Distance(AI.position, transform.position);
                if(Distance <= SpaceBetween)
                {
                    Vector3 direction = transform.position - AI.position;
                    transform.Translate(direction * Time.deltaTime);
                }
            }
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void UpdateSwarmInfo()
    {
        
    }
}
