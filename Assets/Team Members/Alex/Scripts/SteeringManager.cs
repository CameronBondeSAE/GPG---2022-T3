using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Alex
{
    public class SteeringManager : MonoBehaviour
    {
        public SteeringBase currentDirection;


        public void CalculateMoved(SteeringBase newDirection)
        {
            // Check if the state is the same and DON'T swap
            if (newDirection == currentDirection) return;
            // At first 'currentstate' will ALWAYS be null
            if (currentDirection != null) currentDirection.enabled = false;
            newDirection.enabled = true;
            // New state swap over to incoming state
            currentDirection = newDirection;
        }

        public Vector3 CalculateMove(Vector3 movement)
        {
            return movement;
        }
        
    }
}
