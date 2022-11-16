using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marcus
{
    public class DyingState : MonoBehaviour
    {
        public float timer;

        public delegate void Death(float duration);
        public event Death DeathEvent;

        private void Start()
        {
            timer = Random.Range(3f, 4f);
            DeathEvent?.Invoke(timer);
        }

        //Change colour to brown and do other dying things
        private void Update()
        {
            timer -= Time.deltaTime;
            //Call to view for colour change

            if (timer <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
