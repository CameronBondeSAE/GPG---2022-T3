using Unity.Mathematics;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveExplodeState : MonoBehaviour
    {
        public GameObject explosivefragments;
        
        void OnEnable()
        {
            GetComponent<ExplosiveRaycast>().ExplosionRaycast();

            GameObject brokenFragments = Instantiate(explosivefragments, 
                transform.position, quaternion.identity) as GameObject;
            
            //then as we dont need the barrel anymore then just delete it.
            DestroyExplosiveModel();
        }
        void DestroyExplosiveModel()
        {
            Destroy(gameObject);
        }
    }
}
