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

        [SerializeField] public int barrelLength;

        private Flammable flammable;

        private CapsuleCollider capsuleCollider;

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

        public override void OnNetworkSpawn()
        {
	        base.OnNetworkSpawn();

	        if (!IsServer) return;
	        modelView = GetComponentInChildren<FlamethrowerModelView>();

	        modelView.ChangeState += FlipOverheat;

	        isHeld = true;

	        capsuleCollider = GetComponent<CapsuleCollider>();
        }

        private void FlipOverheat(FlamethrowerView.FlamethrowerStates flamethrowerState)
        {
            if (flamethrowerState == 0) overheating = false;
        }

        private void FixedUpdate()
        {
	        if (!IsServer) return;
	        if (overheating) return;
	        if (!shooting && overHeatLevel > 0)
	        {
		        ChangeOverheat(-Time.deltaTime);
	        }
	        if (shooting)
	        {
		        ChangeOverheat(overHeatRate * Time.deltaTime);
	        }
        }

        public void Interact(GameObject interactor)
        {
	        if (isHeld)
	        {
		        if (myType == FlamethrowerType.FireballShooter)
		        {
			        shooting = true;
			        modelView.OnChangeState(FlamethrowerView.FlamethrowerStates.Shooting);
		        }
		        else if (myType == FlamethrowerType.Watercannon)
		        {
			        waterSpraying = true;
			        modelView.OnChangeState(FlamethrowerView.FlamethrowerStates.Shooting);
		        }
	        }
	        else ShootUntilDead();
        }

        public void AltInteract(GameObject interactor)
        {
	        if (!isHeld) return;
	        if (altAmmo <= 0)
	        {   
		        //click!
		        return;
	        }
	        altShooting = true;
	        modelView.OnChangeState(FlamethrowerView.FlamethrowerStates.Shooting);
        }

        public void CancelInteract()
        {
            shooting = false;
            waterSpraying = false;
            modelView.OnChangeState(FlamethrowerView.FlamethrowerStates.Neutral);
        }

        public void CancelAltInteract()
        {
            altShooting = false;
            modelView.OnChangeState(FlamethrowerView.FlamethrowerStates.Neutral);
        }
        
        public void ShootUntilDead()
        {
            shooting = !shooting;
            if(shooting) modelView.OnChangeState(FlamethrowerView.FlamethrowerStates.Shooting);
            else modelView.OnChangeState(FlamethrowerView.FlamethrowerStates.Neutral);
        }

        //FLAMETHROWER WILL OVERHEAT AND EXPLODE IF FIRED TOO MUCH

        public void ChangeOverheat(float amount)
        {
            overHeatLevel += amount;

            if (overHeatLevel < overHeatPoint)
                overheating = false;

            if (overHeatLevel <= 0)
                overHeatLevel = 0;

            if (overHeatLevel >= overHeatPoint && overheating != true)
            {
                overheating = true;
                StartCoroutine(Exploding());
            }
            
            modelView.OnChangeOverheat(overHeatLevel);
        }

        private IEnumerator Exploding()
        {
            modelView.OnChangeState(FlamethrowerView.FlamethrowerStates.Pulsate);
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
            
            capsuleCollider.enabled = false;

            Transform t = transform;
            t.parent = newParent;
            t.rotation = newParent.rotation;
            t.localPosition = new Vector3(0,1,0.5f); //HACK V3 coords
        }

        public void PutDown(GameObject player, ulong networkObjectId)
        {
            isHeld = false;
            RemoveParentClientRpc(networkObjectId);
        }

        [ClientRpc]
        private void RemoveParentClientRpc(ulong networkObjectId)
        {
            //Transform myParent = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(networkObjectId).GetComponent<PlayerController>().playerTransform;
            Transform myParent = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId].transform;
            
            capsuleCollider.enabled = true;

            Transform t = transform;
            t.parent = null;
            t.position = myParent.position + t.forward / 2;
            t.rotation = myParent.rotation;
        }

        private void OnDisable()
        {
            if (IsServer) modelView.ChangeState -= FlipOverheat;
        }
    }
}