using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

namespace Marcus
{
    public class PlantBase : MonoBehaviour, IFlammable, IResource
    {
        public MonoBehaviour burnState;
        
        public Flammable fireness;
        public Health health;

        private void OnEnable()
        {
            fireness.SetOnFireEvent += SetOnFire;
            health.YouDied += Die;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public void ChangeHeat(IHeatSource heatSource, float x)
        {
            // Maybe make a thing to smoke
        }

        public delegate void Burning();
        public event Burning BurningEvent;
        
        void SetOnFire()
        {
            // Fire tween event on view
            BurningEvent?.Invoke();
            GetComponent<StateManager>().ChangeState(burnState);
        }

        void Die(GameObject go)
        {
            Destroy(gameObject);
        }
    }
}
