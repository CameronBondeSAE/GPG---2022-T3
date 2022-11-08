using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class Target : MonoBehaviour
    {
        private Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.layer == LayerMask.NameToLayer("AlienAI"))
            {
                
                Vision vision = collision.gameObject.GetComponent<Vision>();
                vision.enemyInSight.Remove(transform);
                Destroy(gameObject);
            }
        }
    }
}
