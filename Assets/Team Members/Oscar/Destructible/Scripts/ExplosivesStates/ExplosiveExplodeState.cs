using Unity.Mathematics;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveExplodeState : MonoBehaviour
    {
        public GameObject explosivefragments;
        void OnEnable()
        {
            GameObject brokenFragments = Instantiate(explosivefragments, 
                new Vector3(transform.position.x, -1.2f, transform.position.z), quaternion.identity) as GameObject;
            
            //then as we dont need the barrel anymore then just delete it.
            DestroyExplosiveModel();
        }
        void DestroyExplosiveModel()
        {
            Destroy(gameObject);
        }
    }
}
