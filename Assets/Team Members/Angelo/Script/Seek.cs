using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    private float maxSpeed;
    public float maxForce;
    public GameObject targetObject;
    private Vector3 targetPosition;
    private Vector3 velocity;
    private Vector3 acceleration;
    private Vector3 startPosition;

    private Vector3 location;
    // Start is called before the first frame update
    void Start()
    {
        acceleration = Vector3.zero;
        location = transform.position;
        targetPosition = targetObject.transform.position;
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredVelocity = targetPosition - location;
        
        desiredVelocity.Normalize();
        
        desiredVelocity *= maxSpeed;
        
        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - velocity, maxForce);

        acceleration += steer;
        
        velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);
        
        transform.position += velocity * Time.deltaTime;
        
        acceleration = Vector3.zero;
        
        RotateTowardTarget();
        
        transform.position = location;
    }
    
    protected void RotateTowardTarget()
    {
        Vector3 directionToDesiredLocation = location - transform.position;

        directionToDesiredLocation.Normalize();

        float rotZ = Mathf.Atan2(directionToDesiredLocation.y, directionToDesiredLocation.x) * Mathf.Rad2Deg;
        rotZ -= 90;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
