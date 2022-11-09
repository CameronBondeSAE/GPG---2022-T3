using System;
using System.Collections;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveNearlyExplodeState : MonoBehaviour
    {
        public Oscar.StateManager stateManager;
        public MonoBehaviour explodeState;

        public float countDown;
        public event Action AlmostExplode;
        //used IEnumerator so it will start on start but will wait a few seconds before continuing.
        IEnumerator Start()
        {
            AlmostExplode?.Invoke();

            yield return new WaitForSeconds(countDown);
            stateManager.ChangeState(explodeState);
        }
        
    }
}

