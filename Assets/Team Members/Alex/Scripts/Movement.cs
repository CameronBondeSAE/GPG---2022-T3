using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Bson;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;


namespace Alex
{
    public class Movement : MonoBehaviour
    {
        private SteeringBase align;
        private SteeringBase separation;
        private SteeringBase cohesion;
        public Neighbours neighbours;
    
        public void Awake()
        {
            align = GetComponent<Align>();
            separation = GetComponent<Separation>();
            cohesion = GetComponent<Cohesion>();
        }

        Rigidbody rb;
        public float speed;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            rb.AddRelativeForce(0, 0, speed);
        }
        
    }
}
