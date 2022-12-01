using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alex
{
    public class AttackSphereAndShader : MonoBehaviour
    {
        public Transform myTarget;
        public ControllerSwarmer controllerSwarmer;
        private float damagePerSecond = 10;
        public GameObject vFX;

        public LayerMask layerMask;

        private SphereCollider _myCollider;

        public List<Health> _targets;
        

        public void OnEnable()
        {
            _myCollider = GetComponent<SphereCollider>();
            Collider[] overlaps = Physics.OverlapSphere(transform.position, _myCollider.radius);
            
            for (int i = 0; i < overlaps.Length; i++)
            {
	            if ((layerMask.value & (1 << overlaps[i].transform.gameObject.layer)) > 0) //Bitwise ANDing
	            {
		            _targets.Add(overlaps[i].GetComponent<Health>());
		            vFX.SetActive(true);
	            }
            }
            StartCoroutine(DealDamage());
        }

        private void OnTriggerEnter(Collider other)
        {
	        if ((layerMask.value & (1 << other.transform.gameObject.layer)) > 0) //Bitwise ANDing
	        {
		        _targets.Add(other.GetComponent<Health>());
		        vFX.SetActive(true);
	        }
        }

        private void OnTriggerExit(Collider other)
        {
	        if (_targets.Contains(other.GetComponent<Health>())) _targets.Remove(other.GetComponent<Health>());
	        if (_targets.Count == 0) vFX.SetActive(false);
        }

        private IEnumerator DealDamage()
        {
	        yield return new WaitForSeconds(1);
	        int count = _targets.Count;
	        for (int i = 0; i < count; i++)
	        {
		        if (_targets[i] != null)
		        {
			        _targets[i].ChangeHP(-damagePerSecond);
		        }
		        else
		        {
			        _targets.Remove(_targets[i]);
			        i--;
			        count--;
		        }
	        }
	        if (_targets.Count == 0) vFX.SetActive(false);
	        DealDamageLoop();
        }

        private void DealDamageLoop()
        {
	        StartCoroutine(DealDamage());
        }
    }
}

