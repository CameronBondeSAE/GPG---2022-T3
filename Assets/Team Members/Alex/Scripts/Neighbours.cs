using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Alex
{
    public class Neighbours : MonoBehaviour
    {
        public List<Transform> neighbours = new List<Transform>();
        public LayerMask layerMask;

        private void FixedUpdate()
        {
            neighbours.RemoveAll(transformToTest => transformToTest == null);
        }

        public void OnTriggerEnter(Collider other)
        {
            
	        if (neighbours.Contains(other.transform)) return;
            
            if (other.gameObject.layer == LayerMask.NameToLayer("AlienAI"))
                neighbours.Add(other.transform);
            
            if (other.gameObject.layer == LayerMask.NameToLayer("SwarmerAI"))
                neighbours.Add(other.transform);
            
        }

        public void OnTriggerExit(Collider other)
        {
	        if (neighbours.Contains(other.transform)) neighbours.Remove(other.transform);
        }
    }
}