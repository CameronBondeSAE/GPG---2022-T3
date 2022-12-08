using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marcus
{
    public class DyingState : MonoBehaviour
    {
        public float timer;

        public delegate void Death(float duration);
        public event Death DeathEvent;

        private void OnEnable()
        {
	        NetworkManager nm = NetworkManager.Singleton;
	        if (nm.IsServer)
	        {
		        timer = Random.Range(3f, 4f);
		        StartCoroutine(Timer());
	        }
            if(nm.IsClient) DeathEvent?.Invoke(timer);
        }

        //Change colour to brown and do other dying things
        IEnumerator Timer()
        {
	        //Call to view for colour change
	        yield return new WaitForSeconds(timer);
	        Destroy(gameObject);
        }
    }
}
