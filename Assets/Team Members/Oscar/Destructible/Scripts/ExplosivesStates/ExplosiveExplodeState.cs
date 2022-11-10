using Unity.Mathematics;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveExplodeState : MonoBehaviour
    {
        public GameObject explosivefragments;
        public ExplosiveRaycast explosiveRaycast;
        void OnEnable()
        {
            explosiveRaycast.ExplosionRaycast();

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
