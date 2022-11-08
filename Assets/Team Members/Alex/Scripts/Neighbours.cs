using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Alex
{
    public class Neighbours : MonoBehaviour
    {
        public List<GameObject> neighbours = new List<GameObject>();
        public LayerMask layerMask;

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("AlienAI"))
                neighbours.Add(collider.gameObject);
            
            if (collider.gameObject.layer == LayerMask.NameToLayer("SwarmerAI"))
                neighbours.Add(collider.gameObject);
        }

        public void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("AlienAI"))
                neighbours.Remove(collider.gameObject);
            
            if (collider.gameObject.layer == LayerMask.NameToLayer("SwarmerAI"))
                neighbours.Add(collider.gameObject);
        }
    }
}