using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class Disperse : MonoBehaviour
    {
        public Align align;
        public Cohesion cohesion;
        public Separation separation;
        

        void Start()
        {
            StartCoroutine(SwarmersDisperse());
        }
        
        public IEnumerator SwarmersDisperse()
        {
            align.enabled = false;
            cohesion.enabled = false;
            separation.enabled = false;
            yield return new WaitForSeconds(5f);
            align.enabled = true;
            cohesion.enabled = true;
            separation.enabled = true;
            yield return new WaitForSeconds(30f);
            StartCoroutine(SwarmersDisperse());
        }

        
        
    }

    
}
