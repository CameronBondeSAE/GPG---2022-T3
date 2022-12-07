using System.Collections;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveExplodeState : MonoBehaviour, IHeatSource
    {
        [Header("EXPLODE POWER")]
        [SerializeField] private float explodePower;
        [Header("EXPLODE FIRE DAMAGE")] 
        [SerializeField] private float fireDamage;
        [Header("How powerful the upwards force of the explosion is")]
        [SerializeField] private float explodeUpPower;
        [Header("How large the explode circle is")]
        [SerializeField] private float radius;

        private Health health;
        
        [Header("How many explosive fragments spawn")]
        [SerializeField] private int numFragments;
        public GameObject explosivefragments;
        void OnEnable()
        {
            for (int x = 0; x < numFragments; x++)
            {
                if(NetworkManager.Singleton.IsServer) Instantiate(explosivefragments, transform.position, quaternion.identity);
            }
            
           
            //then as we dont need the barrel anymore then just delete it.
            Explode();
        }

        private void Explode()
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider burnVictims in colliders)
            {
                if (burnVictims.GetComponent<Health>() != null)
                {
                    health = burnVictims.GetComponent<Health>();
                    health.ChangeHP(-999999);
                }
                
                Rigidbody rb = burnVictims.GetComponent<Rigidbody>();

                 if (rb != null)
                     rb.AddExplosionForce(explodePower, explosionPos, radius, explodeUpPower);
            }

            DestroyExplosiveModel();
        }
        
        void DestroyExplosiveModel()
        {
            if(NetworkManager.Singleton.IsServer) Destroy(gameObject, .01f);
        }
    }
}
