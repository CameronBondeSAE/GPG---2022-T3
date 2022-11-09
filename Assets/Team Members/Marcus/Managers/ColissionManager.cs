using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Marcus
{
    public class ColissionManager : MonoBehaviour
    {
        public event Action<Collision> OnCollisionEnterEvent;
        public event Action<Collider> OnTriggerEnterEvent;

        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterEvent?.Invoke(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

    }
}
