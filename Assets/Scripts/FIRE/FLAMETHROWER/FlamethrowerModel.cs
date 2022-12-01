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
        private bool canShoot;

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

            isHeld = true;

            canShoot = true;
            
            //TODO: Ollie HACK
            //replace this somehow, it's causing errors on the client
            //GetComponent<NetworkObject>().Spawn();
        }

        private void FixedUpdate()
        {
            HandleOverheat();
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

        private void HandleOverheat()
        {
            if (!shooting)
            {
                overHeatLevel -= 1 * Time.deltaTime;
                if (overHeatLevel <= 0)
                    overHeatLevel = 0;
            }

            if (shooting)
            {
                overHeatLevel += overHeatRate * Time.deltaTime;
            }

            if (overHeatLevel >= overHeatPoint)
            {
                overheating = true;
                DestroySelf();
            }
            
            modelView.OnChangeOverheat(overHeatLevel);
        }

        public void ChangeOverheat(float x)
        {
            overHeatLevel += x;

            if (overHeatLevel < overHeatPoint)
                overheating = false;

            if (overHeatLevel <= 0)
                overHeatLevel = 0;
            
            modelView.OnChangeOverheat(overHeatLevel);
        }
        

        //IPICKUP MANDATORY(S)

        public void PickedUp(GameObject player, ulong localClientId)
        {
            isHeld = true;
            ParentClientRpc(localClientId);
        }

        [ClientRpc]
        public void ParentClientRpc(ulong localClientId)
        {
            //TODO: LUKE needs to fix the client entities knowing about other client entities' PlayerAvatar
            Transform newParent = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(localClientId).GetComponent<PlayerController>().playerTransform;
            
            transform.parent = newParent;
            transform.rotation = newParent.rotation;
            transform.localPosition = new Vector3(0,1,-1.12f); //HACK V3 coords
        }

        public void PutDown(GameObject player, ulong localClientId)
        {
            isHeld = false;
            RemoveParentClientRpc(localClientId);
        }

        [ClientRpc]
        public void RemoveParentClientRpc(ulong localClientId)
        {
            Transform myParent = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(localClientId).GetComponent<PlayerController>().playerTransform;;
            
            transform.parent = null;
            transform.position = myParent.position + (transform.forward / 2);
            transform.rotation = myParent.rotation;
        }

        public void DestroySelf()
        {
            modelView.OnChangeState(2);
        }
    }
}