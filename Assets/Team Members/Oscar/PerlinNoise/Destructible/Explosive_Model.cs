using UnityEngine;

namespace Oscar
{
    public class Explosive_Model : MonoBehaviour, IFlammable
    {
        public enum States
        {
            Idle,
            AboutToBlow,
            Explode
        }
        
        public States currentStates;
        
        private void Update()
        {
            switch (currentStates)
            {
                case States.Idle:
                    //stationary Explosive
                    break;
                case States.AboutToBlow:
                    //when the Explosive is pulsating and about to explode
                    break;
                case States.Explode:
                    //when the explosive has exploded/in the midst of it
                    break;
            }
        }
        
        
        
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
        public void SetOnFire()
        {
            this.GetComponent<Renderer>().material.color = Color.red;
            onFire = true;
            
            print("ow");
        }
    
        
    }
}

