using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lloyd
{
    public class Flamethrower : MonoBehaviour, IPickup, IInteractable
    {
        [Header("FLAME SETTINGS [DAMAGE / SIZE / FIRE RATE]")] [SerializeField]
        private float fireDamage;

        [SerializeField]
        public enum FlamethrowerType
        {
            FireballShooter,
            OverlapBoxFire,
            FunnyThirdKind
        };

        private FlamethrowerType myType;

        [Header("FLAME PREFAB")] private GameObject firePrefab;

        private Rigidbody firePrefabRb;

        private Rigidbody firePointRb;
        private Vector3 firePointPos;

        private Coroutine shootCo;

        [SerializeField] private float force;

        public GameObject fireball;

        [SerializeField] private float fireRate;

        [SerializeField] private float altFireRate;

        [SerializeField] private float wobbleMultiplier;
        [SerializeField] private float xScale;

        public bool isHeld { get; set; }
        
        public bool locked { get; set; }
        
        public bool autoPickup { get; set; }

        private Rigidbody rb;

        private float angle;

        private Vector3 angleVector;

        Quaternion currentRotation;
        
        //am I allowed to shoot? ticks depending on fire rate and ammo
        private bool canShoot;

        [Header("OVERHEAT STATS")]
        
        [SerializeField] private float overHeatRate;
        [SerializeField] private float overHeatPoint;
        public float overHeatLevel;

        private bool shooting;

        public FlamethrowerModelView modelView;

        private void OnEnable()
        {
            //modelView = GetComponentInChildren<FlamethrowerModelView>(); 
            
            isHeld = false;
            
            canShoot = true;

            rb = GetComponent<Rigidbody>();

            firePointRb = GetComponentInChildren<Rigidbody>();

            StartCoroutine(SpitFire());
        }

        private void FixedUpdate()
        {
            if (shooting)
            {
                Wobble();
            }

            HandleOverheat();
            
           // Debug.Log(overHeatLevel);
        }
        
        public void Interact(GameObject interactor)
        {
                if(isHeld)
                ShootFire();

                else if (!isHeld)
                ShootTilDead();
        }

        //FLAMETHROWER 
        //
        
        public void ShootFire()
        {
         //   if (canShoot && isHeld)
         if(canShoot)
                StartCoroutine(SpitFire());

           // if (canShoot && !isHeld)
             //   StartCoroutine(ShootTilDead());
        }

        private IEnumerator SpitFire()
        {
                shooting = true;
                
                canShoot = false;
                
                firePointPos = firePointRb.transform.position;
                Vector3 targetDir = firePointPos - transform.position;
                
                GameObject _fire = Instantiate(fireball, transform.position, Quaternion.identity) as GameObject;
                firePrefabRb = _fire.GetComponent<Rigidbody>();
                firePrefabRb.AddForce(targetDir * force, ForceMode.Impulse);

                yield return new WaitForSecondsRealtime(fireRate);

                canShoot = true;
                shooting = false;
        }

        private IEnumerator ShootTilDead()
        {
            while (!isHeld)
            {
                StartCoroutine(SpitFire());
                yield return new WaitForSeconds(fireRate);
            }
        }
        
        //ALT FIRE GOES HERE
   public void ShootAltFire()
        {
            if (canShoot)
                StartCoroutine(AltFire());
        }
   
        private IEnumerator AltFire()
        {
            canShoot = false;
            
            yield return new WaitForSecondsRealtime(altFireRate);

            canShoot = true;
        }
        
        //FLAMETHROWER WILL OVERHEAT AND EXPLODE IF FIRED TOO MUCH

        private void HandleOverheat()
        {
            if (!shooting)
            {
                overHeatLevel--;
                if (overHeatLevel <= 0)
                    overHeatLevel = 0;
            }

            if (shooting)
            {
                 overHeatLevel += overHeatRate * Time.deltaTime;
            }
            
            modelView.OnChangeOverheat(overHeatLevel);
            
            if (overHeatLevel >= overHeatPoint)
                StartCoroutine(Explode());
        }

        private IEnumerator Explode()
        {
            canShoot = false;
            canShoot = false;
            modelView.OnYouDied();
            //tween, wait and explode
            yield break;
        }
        
        //IPICKUP MANDATORY(S)

        public void PickedUp(GameObject player)
        {
            isHeld = true;
            transform.SetParent(player.transform);
        }

        public void PutDown(GameObject player)
        {
            isHeld = false;
        }
        public void DestroySelf()
                 {
                     StartCoroutine(Explode());
                 }

        //PERLIN WOBBLE EXPERIMENT
        private void Wobble()
        {
            float height = wobbleMultiplier * Mathf.PerlinNoise(Time.time * xScale, 0.0f);

            Vector3 _angleVector = new Vector3(0, height, 0);

            currentRotation.eulerAngles = _angleVector;

            firePointRb.MoveRotation(Quaternion.AngleAxis(height, _angleVector));
        }
    }
}