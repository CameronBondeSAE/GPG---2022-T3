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

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("AlienAI"))
                neighbours.Add(collider.transform);
            
            if (collider.gameObject.layer == LayerMask.NameToLayer("SwarmerAI"))
                neighbours.Add(collider.transform);
        }

        public void OnTriggerExit(Collider collider)
        {
            neighbours.Remove(collider.transform);
        }
    }
}