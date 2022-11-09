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
        public void SpawnTheBase(Vector3 prefabPosition, GameObject HQParent)
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
                    //spawn the bases
                    Vector3 tempBasePos = prefabPosition;
                    prevBasePos = tempBasePos;
                    
                    GameObject HQ = Instantiate(Base, prefabPosition, quaternion.identity);
                    
                    HQ.transform.SetParent(HQParent.transform);
                    HQAmount++;
                }
            }
        }

    }

}