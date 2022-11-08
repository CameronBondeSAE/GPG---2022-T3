using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lloyd
{
    public class Flamethrower : MonoBehaviour
    {
        [Header("FLAME SETTINGS [DAMAGE / SIZE / FIRE RATE]")] [SerializeField]
        private float _heatDamage;

        [SerializeField] private Vector3 _boxExtents;

        [SerializeField] private float _boxWidth;

        [SerializeField]
        private enum FlamethrowerType
        {
            FireballShooter,
            OverlapBoxFire,
            FunnyThirdKind
        };

        private FlamethrowerType _myType;

        private Vector3 _boxCenter;

        [Header("FLAME PREFAB")] private GameObject _firePrefab;

        private Rigidbody _firePrefabRb;

        [SerializeField] private float _force;

        public GameObject _fireball;

        [SerializeField] private float _fireRate;

        [SerializeField] private float _altFireRate;

        [SerializeField] private int _wobbleMultiplier;

        [SerializeField] private float _maxFuel;
        private float _fuel;

        private Rigidbody _rb;

        private float _angle;

        private Vector3 _angleVector;

        Quaternion _currentRotation;

        // "Bobbing" animation from 1D Perlin noise.

        // Range over which height varies.
        [SerializeField] float _perlinY;

        // Distance covered per second along X axis of Perlin plane.
        [SerializeField] float _perlinX;

        //am I allowed to shoot? ticks depending on fire rate and ammo
        private bool _canShoot;

        //am I currently shooting? Shoot() creates a box collider that increases heat
        private bool _shooting;

        private float _distance;

        private float _minDistance;

        private float _proximityMultiplier;

        private Vector3 _burnVictimPos;

        private void OnEnable()
        {
            _canShoot = true;
            _fuel = _maxFuel;

            _rb = GetComponent<Rigidbody>();
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
            _angle = _perlinY * Mathf.PerlinNoise(Time.time * _perlinX, 0f);

            _angleVector = new Vector3(_angle * _wobbleMultiplier, _angle * _wobbleMultiplier, 0);

            _currentRotation.eulerAngles = _angleVector;
        }

        public void ShootFire()
        {
            if (_canShoot)
                StartCoroutine(SpitFire());
        }

        private IEnumerator SpitFire()
        {
            if (_fuel > 0)
            {
                _fuel--;
                
                _canShoot = false;
                _shooting = true;

                GameObject _fire = Instantiate(_fireball, transform.position, _currentRotation) as GameObject;
                _firePrefabRb = _fire.GetComponent<Rigidbody>();
                _firePrefabRb.AddForce(transform.forward * _force, ForceMode.Impulse);

                yield return new WaitForSecondsRealtime(_fireRate);

                _canShoot = true;
                _shooting = false;
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
            _shooting = true;


            yield return new WaitForSecondsRealtime(_altFireRate);

            _canShoot = true;
            _shooting = false;
        }


        private void FixedUpdate()
        {
            //transform.Rotate(0.0f, .5f, 0.0f, Space.Self);

            Wobble();
        }
    }
}