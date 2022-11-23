using System.Collections;
using System.Collections.Generic;
using Lloyd;
using Unity.Mathematics;
using UnityEngine;

namespace Oscar
{
    public class SpawnBases : MonoBehaviour
    {
        private Lloyd.HQ hqScript;

        private Vector3 prevBasePos;
        
        [SerializeField] private float minDist = 30;
        
        public int HQAmount;
        public GameObject humanBase;
        public GameObject alienBase;
        public float tempBaseDist;
        private bool humanHQ = true;
        
        [SerializeField] private float destroyRadius;


        public void SpawnTheBase(List<Vector3> prefabPosition, GameObject HQParent)
        {
            //minDist = 90 - (amount / 2);
            //calculate distances between potential created object and the previously spawned one
            for (int i = 0; i < prefabPosition.Count; i++)
            {
                tempBaseDist = Vector3.Distance(prefabPosition[i], prevBasePos);
                //if the distance is less then the minimum distance set 
                if (tempBaseDist > minDist)
                {
                    //and there isnt already 2 bases
                    if (HQAmount <= 1)
                    {
                        if (humanHQ == true)
                        {
                            //spawn the bases
                            Vector3 tempBasePos = prefabPosition[i];
                            
                            prevBasePos = tempBasePos;
                            
                            GameObject HQ = Instantiate(humanBase, prefabPosition[i], quaternion.identity);
                            
                            HQ.transform.SetParent(HQParent.transform);
                            HQAmount++;
                            humanHQ = false;
                            
                            hqScript = humanBase.GetComponentInChildren<HQ>();

                            hqScript.DestroyLand(destroyRadius);
                        }
                        else
                        {
                            //spawn the bases
                            Vector3 tempBasePos = prefabPosition[i];
                            
                            prevBasePos = tempBasePos;
                            
                            GameObject HQ = Instantiate(alienBase, prefabPosition[i], quaternion.identity);
                            
                            HQ.transform.SetParent(HQParent.transform);
                            HQAmount++;
                            humanHQ = true;
                            
                            hqScript = alienBase.GetComponentInChildren<HQ>();

                            hqScript.DestroyLand(destroyRadius);
                        }
                    }
                }
            }
        }
    }
}