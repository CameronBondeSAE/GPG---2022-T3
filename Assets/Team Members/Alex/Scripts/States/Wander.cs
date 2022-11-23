using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

namespace Alex
{
    public class Wander : MonoBehaviour
    {
        public float perlinNoise;
        Rigidbody rb;
        public int offset;
        public int turnForce;
        public float randomOffSet;
        

        // Start is called before the first frame update
        void Start()
        {
            randomOffSet = Random.Range(1, 1000);
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            perlinNoise = Mathf.PerlinNoise(Time.time + randomOffSet, 0) * 2 - 1;

            rb.AddRelativeTorque(0, perlinNoise * turnForce, 0);
        }
    }
}