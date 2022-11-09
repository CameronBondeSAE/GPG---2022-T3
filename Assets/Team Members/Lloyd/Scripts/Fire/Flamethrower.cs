using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lloyd
{
    public class Flamethrower : MonoBehaviour, IPickup
    {
        [Header("FLAME SETTINGS [DAMAGE / SIZE / FIRE RATE]")] [SerializeField]
        private float _heatDamage;

        [SerializeField] private Vector3 _boxExtents;

        [SerializeField] private float _boxWidth;

        [SerializeField]
        public enum FlamethrowerType
        {
            FireballShooter,
            OverlapBoxFire,
            FunnyThirdKind
        };

        private FlamethrowerType _myType;

        private Vector3 _boxCenter;

        [Header("FLAME PREFAB")] private GameObject _firePrefab;

        private Rigidbody _firePrefabRb;

        private Rigidbody _firePointRb;
        private Vector3 _firePointPos;

        [SerializeField] private float _force;

        public GameObject _fireball;

        [SerializeField] private float _fireRate;

        [SerializeField] private float _altFireRate;

        [SerializeField] private float _wobbleMultiplier;
        [SerializeField] private float _xScale;

        [SerializeField] private float _maxFuel;
        [SerializeField] private float _fuel;

        private bool _equipped;

        private Rigidbody _rb;

        private float _angle;

        private Vector3 _angleVector;

        Quaternion _currentRotation;
        
        //am I allowed to shoot? ticks depending on fire rate and ammo
        private bool _canShoot;

        private float _distance;

        private float _minDistance;

        private float _proximityMultiplier;

        private Vector3 _burnVictimPos;

        private void OnEnable()
        {
            _canShoot = true;
            _fuel = _maxFuel;

            _rb = GetComponent<Rigidbody>();

            _firePointRb = GetComponentInChildren<Rigidbody>();
        }

        private void Shoot()
        {
            Collider[] hitColliders = Physics.OverlapBox(_boxCenter, _boxWidth * _boxExtents);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.GetComponent<IFlame>() != null)
                {
                    hitCollider.GetComponent<IFlame>().ChangeHeat(_heatDamage);

                    _burnVictimPos = hitCollider.transform.position;

                    GameObject fire = Instantiate(_firePrefab, _burnVictimPos, Quaternion.identity) as GameObject;

                    _distance = Vector3.Distance(_boxCenter, _burnVictimPos);
                    if (_distance > _minDistance)
                    {
                        hitCollider.GetComponent<IFlame>().ChangeHeat(_heatDamage * _proximityMultiplier);
                    }
                }
            }
        }


        private void Wobble()
        {
            float height = _wobbleMultiplier * Mathf.PerlinNoise(Time.time * _xScale, 0.0f);

            Vector3 _angleVector = new Vector3(0, height, 0);

            _currentRotation.eulerAngles = _angleVector;

            _firePointRb.MoveRotation(Quaternion.AngleAxis(height, _angleVector));
        }

        public void ShootFire()
        {
            if (_canShoot)
                StartCoroutine(SpitFire());
        }

        private IEnumerator SpitFire()
        {
            if (_equipped && _fuel > 0)
            {
                _fuel--;
                
                _canShoot = false;
                
                _firePointPos = _firePointRb.transform.position;
                Vector3 targetDir = _firePointPos - transform.position;
                
                GameObject _fire = Instantiate(_fireball, transform.position, Quaternion.identity) as GameObject;
                _firePrefabRb = _fire.GetComponent<Rigidbody>();
                _firePrefabRb.AddForce(targetDir * _force, ForceMode.Impulse);

                yield return new WaitForSecondsRealtime(_fireRate);

                _canShoot = true;
            }
        }

        public void ShootAltFire()
        {
            if (_canShoot)
                StartCoroutine(AltFire());
        }

        private IEnumerator AltFire()
        {
            _canShoot = false;


            yield return new WaitForSecondsRealtime(_altFireRate);

            _canShoot = true;
        }


        private void FixedUpdate()
        {
            //transform.Rotate(0.0f, .5f, 0.0f, Space.Self);

            Wobble();
        }

        public void PickUp(GameObject player)
        {
            _equipped = true;
            transform.SetParent(player.transform);
        }

        public void Throw()
        {
            _equipped = false;
        }
    }
}