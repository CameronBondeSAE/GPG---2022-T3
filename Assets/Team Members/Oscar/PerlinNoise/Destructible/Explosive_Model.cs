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

        private float TimeRemaining = 3;
    
        private bool onFire = false;
        private float myHealth = 100;
    
        public delegate void OnFire();
        public event OnFire burning;
        
        
        
        public BarrelHealth barrelHealth;
        public void Start()
        {
            barrelHealth.myAmount(myHealth);
        }
    }
}

