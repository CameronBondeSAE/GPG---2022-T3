using System;
using UnityEngine;

namespace Oscar
{
    public class Explosive_Model : MonoBehaviour
    {
        public enum ExplosionStates
        {
            Idle,
            AboutToBlow,
            Explode
        }
        
        public ExplosionStates state;
        public ExplosionStates lastState;
    }
}

