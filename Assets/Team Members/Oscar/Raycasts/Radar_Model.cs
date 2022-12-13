using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using Shapes;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

namespace Oscar
{
    public class Radar_Model : NetworkBehaviour, IFlammable,IPickupable
    {
        //is it on the player or not
        private bool radarOn = false;

        //for the actual raycast
        public float timer;
        private float radarSpeed = 100f;
        public Vector3 dir;

        public RaycastHit hit;

        private CapsuleCollider capsuleCollider;
        
        [SerializeField]private float length = 10f;

        private void OnEnable()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        void Update()
        {
            //create the loop for the radar using time.deltatime
            timer += Time.deltaTime * radarSpeed;
            if (timer >= 360f)
            { 
                timer = 0f;
            }
            
            //defined direction over time
            dir = Quaternion.Euler(0, timer, 0) * transform.forward * length;
            
            //the actual raycast that will read the collisions if there are any
            Ray ray = new Ray(transform.position, dir);
            if (radarOn == true)
            {
                if (Physics.Raycast(ray, out hit, length))
                {
	                IAffectedByVisibility[] affectedByVisibilities = hit.collider.GetComponents<IAffectedByVisibility>();
	                if (affectedByVisibilities != null)
                    {
	                    foreach (var visibility in affectedByVisibilities)
	                    {
		                    visibility.Detection(1);
	                    }
                    }
                }
            }
            transform.rotation = Quaternion.identity;
        }

        public event Action<bool> RadarOnNow;
        
        //Interfaces that Interact with this item.
        public bool isHeld { get; set; }
        public bool locked { get; set; }

        public bool autoPickup
        {
            get
            {
                return false;
            }
            set { }
        }
        
        public bool RadarSwitchOn()
        {
            if (radarOn == false)
            {
                radarOn = true;
            }
            RadarOnNow?.Invoke(radarOn);
            return radarOn;
        }

        public bool RadarSwitchOff()
        {
            if (radarOn == true)
            {
                radarOn = false;
            }
            RadarOnNow?.Invoke(radarOn);
            return radarOn;
        }

        public void PickedUp(GameObject interactor, ulong networkObjectId)
        {
            RadarSwitchOn();
            isHeld = true;
            
            ParentClientRpc(networkObjectId);
        }
        
        [ClientRpc]
        public void ParentClientRpc(ulong networkObjectId)
        {
            Transform newParent = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId].transform;
            
            capsuleCollider.enabled = false;

            Transform t = transform;
            t.parent = newParent;
            t.rotation = newParent.rotation;
            t.localPosition = new Vector3(0,1,0f); //HACK V3 coords
        }

        public void PutDown(GameObject interactor, ulong networkObjectId)
        {
            RadarSwitchOff();
            isHeld = false;
            RemoveParentClientRpc(networkObjectId);
        }

        [ClientRpc]
        public void RemoveParentClientRpc(ulong networkObjectId)
        {
            Transform myParent = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId].transform;
            capsuleCollider.enabled = true;

            Transform t = transform;
            t.parent = null;
            t.position = myParent.position + (transform.forward / 2);
            t.rotation = myParent.rotation;
        }

        public void DestroySelf()
        {
            
        }
        
        public void ChangeHeat(IHeatSource heatSource, float x)
        {
            
        }
    }
}

