using Unity.Mathematics;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveExplodeState : MonoBehaviour
    {
        public GameObject explosivefragments;

        private float transformRotation;
        // Start is called before the first frame update
        void Start()
        {
            //play particle system
            
            //spawn the fragments of the explosives remains
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
