using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oscar
{
    public class Base_Prefab : MonoBehaviour
    {
        void Start()
        {
            //delete items with health when spawned
            //should delete items that overlap with the hq
            Collider[] obstructions = Physics.OverlapSphere(transform.position, 5f);
            foreach (Collider item in obstructions)
            {
                if (item.GetComponent<Health>() != null)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }
}

