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

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.GetComponent<Energy>() != null && !collider.isTrigger)
                neighbours.Add(collider.gameObject);
        }

        public void OnTriggerExit(Collider collider)
        {
            if (collider.GetComponent<Energy>() != null)
                neighbours.Remove(collider.gameObject);
        }
    }
}