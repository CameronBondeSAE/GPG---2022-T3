using System;
using System.Collections;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveNearlyExplodeState : MonoBehaviour
    {
        public Oscar.StateManager stateManager;
        public MonoBehaviour explodeState;
        public MonoBehaviour idleState;

        private bool explode = true;

        public float countDown;
        public event Action AlmostExplode;

        private Flammable flammable;
        private void OnEnable()
        {
            // flammable = GetComponent<Flammable>();
            // flammable.CoolDown += FireOff;
            
            StartCoroutine(ExplodeCountdown());
        }
        
        //used IEnumerator so it will start on start but will wait a few seconds before continuing.
        IEnumerator ExplodeCountdown()
        {
            AlmostExplode?.Invoke();

            yield return new WaitForSeconds(countDown);

            if (explode)
            {
                stateManager.ChangeState(explodeState);
            }
            else
            {
                stateManager.ChangeState(idleState);
            }
        }
        
        void FireOff()
        {
            explode = false;
        }
    }
}

