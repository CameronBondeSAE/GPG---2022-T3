using System;
using System.Collections;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveNearlyExplodeState : MonoBehaviour
    {
        public float countDown;
        public MonoBehaviour explodeState;
        public event Action AlmostExplode;
        //used IEnumerator so it will start on start but will wait a few seconds before continuing.
        IEnumerator Start()
        {
            AlmostExplode?.Invoke();

            yield return new WaitForSeconds(countDown);
            GetComponent<Oscar.StateManager>().ChangeState(explodeState);
        }
        
    }
}

