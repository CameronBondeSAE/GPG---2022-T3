using UnityEngine;


namespace Alex
{
    public class Movement : MonoBehaviour
    {
        private SteeringBase align;
        private SteeringBase separation;
        private SteeringBase cohesion;
    
        public void Awake()
        {
            align = GetComponent<Align>();
            separation = GetComponent<Separation>();
            cohesion = GetComponent<Cohesion>();
        }

        Rigidbody rb;
        public float speed;
        public float slideTowardsSpeed;

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
