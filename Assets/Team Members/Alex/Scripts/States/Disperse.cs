using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

namespace Alex
{
    public class Disperse : MonoBehaviour
    {
        public Align align;
        public Cohesion cohesion;
        public Separation separation;
        public Neighbours neighbours;
        public float offTime = 5f;
        public float onTime = 15f;


        void Start()
        {
            StartCoroutine(SwarmersDisperse());
        }
        
        public IEnumerator SwarmersDisperse()
        {
            neighbours.neighbours.Clear();
            neighbours.enabled = false;
            align.enabled = false;
            cohesion.enabled = false;
            separation.enabled = false;
            yield return new WaitForSeconds(onTime);
            neighbours.enabled = true;
            align.enabled = true;
            cohesion.enabled = true;
            separation.enabled = true;
            yield return new WaitForSeconds(offTime);
            StartCoroutine(SwarmersDisperse());
        }

        
        
    }

    
}
