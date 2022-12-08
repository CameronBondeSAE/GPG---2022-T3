using System;
using System.Collections;
using System.Collections.Generic;
using Luke;
using Tanks;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lloyd
{
    public class FlamethrowerModel : NetworkBehaviour, IPickupable, IInteractable, IHeatSource
    {
        [Header("FLAME SETTINGS [DAMAGE / SIZE / FIRE RATE]")] [SerializeField]
        public float fireDamage;
        public enum FlamethrowerType
        {
            FireballShooter,
            Watercannon,
            FunnyThirdKind
        };

        public FlamethrowerType myType;

        [Header("FLAME PREFAB")] public GameObject fireball;

        [Header("WaterPrefab")] public GameObject waterball;

        [SerializeField] public float force;

        [SerializeField] public float fireRate;

        [Header("ALT FIRE")] public GameObject barrel;

        [SerializeField] public int altAmmo;

        [SerializeField] public float altForce;

        [SerializeField] public float altFireRate;

        private Flammable flammable;

        private SphereCollider sphereCollider;

        [SerializeField] public float countDownTimer;

        public bool isHeld { get; set; }

        public bool locked { get; set; }

        public bool autoPickup
        {
	        get
	        {
		        return false;
	        }
	        set
	        {
		        
	        }
        }

        private float angle;

        private Vector3 angleVector;

        Quaternion currentRotation;

        //am I allowed to shoot? ticks depending on fire rate and ammo
        //private bool canShoot;

        [Header("OVERHEAT STATS")] [SerializeField]
        public float overHeatRate;

        [SerializeField] public float overHeatPoint;
        public float overHeatLevel;

        public bool overheating;

        [SerializeField] public float explodeTimer;

        public bool shooting=false;

        public bool waterSpraying = false;

        public bool altShooting=false;

        public FlamethrowerModelView modelView;
        
        private void OnEnable()
        {
            modelView = GetComponentInChildren<FlamethrowerModelView>();

            modelView.ChangeState += FlipOverheat;

            isHeld = true;

            sphereCollider = GetComponent<SphereCollider>();

            //TODO: Ollie HACK
            //replace this somehow, it's causing errors on the client
            //GetComponent<NetworkObject>().Spawn();
        }
        
        private void FlipOverheat(int x)
        {
            if (x == 0)
                overheating = false;
        }

        private void FixedUpdate()
        {
            if (!overheating)
            {
                if (!shooting && overHeatLevel > 0)
                {
                    ChangeOverheat(-1 * Time.deltaTime);
                }

                if (shooting)
                {
                    ChangeOverheat(overHeatRate * Time.deltaTime);
                }
            }
        }

        public void Interact(GameObject interactor)
        {
                if (isHeld)
                    if (myType == FlamethrowerType.FireballShooter)
                    {
                        shooting = true;
                        modelView.OnChangeState(1);
                    }
                    else if (myType == FlamethrowerType.Watercannon)
                    {
                        waterSpraying = true;
                        modelView.OnChangeState(1);
                    }

                else
                    ShootUntilDead();
        }

        public void AltInteract(GameObject interactor)
        {
            if (isHeld)
            {
                if (altAmmo <= 0)
                {   
                    //click!
                    return;
                }
                altShooting = true;
                modelView.OnChangeState(1);
            }
        }

        public void CancelInteract()
        {
            shooting = false;
            waterSpraying = false;
            modelView.OnChangeState(0);
        }

        public void CancelAltInteract()
        {
            altShooting = false;
            modelView.OnChangeState(0);
        }
        
        public void ShootUntilDead()
        {
            shooting = !shooting;
            if(shooting)
                modelView.OnChangeState(1);
            
            else modelView.OnChangeState(0);
        }

        //FLAMETHROWER WILL OVERHEAT AND EXPLODE IF FIRED TOO MUCH

        public void ChangeOverheat(float x)
        {
            overHeatLevel += x;

            if (overHeatLevel < overHeatPoint)
                overheating = false;

            if (overHeatLevel <= 0)
                overHeatLevel = 0;

            if (overHeatLevel >= overHeatPoint)
            {
                overheating = true;
                StartCoroutine(Exploding());
            }
            
            modelView.OnChangeOverheat(overHeatLevel);
        }

        private IEnumerator Exploding()
        {
            modelView.OnChangeState(2);
            yield return new WaitUntil(() => overheating == false);
        }

        public void DestroySelf()
        {
            overHeatLevel = 1000f;
            overheating = true;
            StartCoroutine(Exploding());
        }

        //IPICKUP MANDATORY(S)

        public void PickedUp(GameObject player, ulong networkObjectId)
        {
            isHeld = true;
            
            ParentClientRpc(networkObjectId);
        }

        [ClientRpc]
        public void ParentClientRpc(ulong networkObjectId)
        {
            //TODO: LUKE needs to fix the client entities knowing about other client entities' PlayerAvatar
            //also an issue with the FT not following the client because of NetworkTransform?

            //Transform newParent = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(networkObjectId).GetComponent<PlayerController>().playerTransform;
            Transform newParent = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId].transform;
            
            sphereCollider.enabled = false;
            
            transform.parent = newParent;
            transform.rotation = newParent.rotation;
            transform.localPosition = new Vector3(0,1,-1.12f); //HACK V3 coords
        }

        public void PutDown(GameObject player, ulong networkObjectId)
        {
            isHeld = false;
            RemoveParentClientRpc(networkObjectId);
        }

        [ClientRpc]
        public void RemoveParentClientRpc(ulong networkObjectId)
        {
            //Transform myParent = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(networkObjectId).GetComponent<PlayerController>().playerTransform;
            Transform myParent = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId].transform;
            
            sphereCollider.enabled = true;
            
            transform.parent = null;
            transform.position = myParent.position + (transform.forward / 2);
            transform.rotation = myParent.rotation;
        }

        private void OnDisable()
        {
            modelView.ChangeState -= FlipOverheat;
        }
    }
}