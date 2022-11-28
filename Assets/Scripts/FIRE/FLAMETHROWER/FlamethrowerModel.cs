using System;
using System.Collections;
using System.Collections.Generic;
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
        
        [SerializeField]
        public enum FlamethrowerType
        {
            FireballShooter,
            OverlapBoxFire,
            FunnyThirdKind
        };

        private FlamethrowerType myType;

        [Header("FLAME PREFAB")] public GameObject fireball;

        [Header("WaterPrefab")] public GameObject waterball;

        [SerializeField] public float force;

        [SerializeField] public float fireRate;

        [Header("ALT FIRE")] public GameObject barrel;

        [SerializeField] private int altAmmo;

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
            
           // GetComponent<NetworkObject>().Spawn();
            }

        private void FixedUpdate()
        {
            HandleOverheat();
        }

        public void Interact(GameObject interactor)
        {
            if(isHeld)
                ShootFire();

            else
                ShootUntilDead();
        }
        
        public void ShootUntilDead()
        {
            shooting = !shooting;
            if(shooting)
                modelView.OnChangeState(1);
            
            else modelView.OnChangeState(0);
        }

        public void ShootFire()
        {
            shooting = !shooting;
            if(shooting)
            modelView.OnChangeState(1);
            
            else modelView.OnChangeState(0);
        }

        public void SprayWater()
        {
            waterSpraying = !waterSpraying;
            if (waterSpraying)
            {
                modelView.OnChangeState(1);
            }
            else
            {
                modelView.OnChangeState(0);
            }
        }

        public void ShootAltFire()
        {
            altShooting = !altShooting;
            if (altShooting && altAmmo > 0)
            {
                modelView.OnChangeState(1);
                altAmmo--;
            }
            else modelView.OnChangeState(0);
        }

        //FLAMETHROWER WILL OVERHEAT AND EXPLODE IF FIRED TOO MUCH

        private void HandleOverheat()
        {
            if (!shooting && !waterSpraying)
            {
                overHeatLevel -= 1 * Time.deltaTime;
                if (overHeatLevel <= 0)
                    overHeatLevel = 0;
            }

            if (shooting)
            {
                overHeatLevel += overHeatRate * Time.deltaTime;
            }

            modelView.OnChangeOverheat(overHeatLevel);

            if (overHeatLevel >= overHeatPoint)
            {
                DestroySelf();
            }

            if (waterSpraying)
            {
                overHeatLevel -= overHeatRate * Time.deltaTime;
            }
        }
        

        //IPICKUP MANDATORY(S)

        public void PickedUp(GameObject player)
        {
            isHeld = true;
        }

        public void PutDown(GameObject player)
        {
            isHeld = false;
        }

        public void DestroySelf()
        {
            overheating = true;
            modelView.OnChangeState(2);
        }
    }
}