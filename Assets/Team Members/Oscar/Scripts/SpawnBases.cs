using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Oscar
{
    public class SpawnBases : MonoBehaviour
    {
        private Vector3 prevBasePos;
        [SerializeField]
        private float minDist = 30;
        public int HQAmount;
        public GameObject Base;
        public float tempBaseDist;

        //work on this
        public void SpawnTheBase(Vector3 prefabPosition, GameObject HQParent, float perlinValue)
        {
            if (perlinValue < .4f && prefabPosition.x > 20 && prefabPosition.z > 20)
            {
                //minDist = 90 - (amount / 2);
                //calculate distances between potential created object and the previously spawned one
                tempBaseDist = Vector3.Distance(prefabPosition, prevBasePos);

                //if the distance is less then the minimum distance set 
                if (tempBaseDist > minDist)
                {
                    //and there isnt already 2 bases
                    if (HQAmount <= 1)
                    {
                        //delete items with health when spawned
                        //should delete items that overlap with the hq
                        Collider[] obstructions = Physics.OverlapSphere(transform.position, 10f);
                        foreach (Collider item in obstructions)
                        {
                            if (item.GetComponent<Marcus.Health>() != null)
                            {
                                Destroy(item.gameObject);
                            }
                        }
                        //spawn the bases
                        Vector3 tempBasePos = prefabPosition;
                        prevBasePos = tempBasePos;
                        GameObject HQ = Instantiate(Base, prefabPosition, quaternion.identity);
                        HQ.name = "base";
                        HQ.transform.SetParent(HQParent.transform);
                        HQAmount++;

                    }
                }
            }
        }
    }

}