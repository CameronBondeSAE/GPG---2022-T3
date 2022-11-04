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
        
        [SerializeField] private int _wobbleMultiplier;

        private Rigidbody _rb;

        private float _angle;

        private Vector3 _angleVector;
        
        Quaternion _currentRotation;
        
        // "Bobbing" animation from 1D Perlin noise.

        // Range over which height varies.
        [SerializeField] float _perlinHeight = 1.0f;

        // Distance covered per second along X axis of Perlin plane.
       [SerializeField] float _perlinX = 1.0f;

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
            _angle = _perlinHeight * Mathf.PerlinNoise(Time.time * _perlinX, 0.0f);
            
            _angleVector = new Vector3(_angle*_wobbleMultiplier, _angle*_wobbleMultiplier, 0);
            
            _currentRotation.eulerAngles = _angleVector;
        }

        public void ShootFire()
        {
            if(_canShoot)
            StartCoroutine(SpitFire());
        }

        private IEnumerator SpitFire()
        {
            _canShoot = false;
            _shooting = true;
            
            GameObject _fire = Instantiate(_fireball, transform.position, _currentRotation) as GameObject;
            _firePrefabRb = _fire.GetComponent<Rigidbody>();
            _firePrefabRb.AddForce(transform.forward * _force, ForceMode.Impulse);
            
            GameObject _fire1 = Instantiate(_fireball, transform.position, _currentRotation) as GameObject;
            _firePrefabRb = _fire1.GetComponent<Rigidbody>();
            _firePrefabRb.AddForce(transform.forward * (_force/(_angle*2f)), ForceMode.Impulse);
            
            GameObject _fire2 = Instantiate(_fireball, transform.position, _currentRotation) as GameObject;
            _firePrefabRb = _fire2.GetComponent<Rigidbody>();
            _firePrefabRb.AddForce(transform.forward * (_force/(_angle*3f)), ForceMode.Impulse);

            int rand = Random.Range(1, 10);

            if (rand < 5)
            {

                GameObject _fire4 = Instantiate(_fireball, transform.position, _currentRotation) as GameObject;
                _firePrefabRb = _fire4.GetComponent<Rigidbody>();
                _firePrefabRb.AddForce(transform.forward * (_force / (_angle * 4f)), ForceMode.Impulse);

                GameObject _fire5 = Instantiate(_fireball, transform.position, _currentRotation) as GameObject;
                _firePrefabRb = _fire5.GetComponent<Rigidbody>();
                _firePrefabRb.AddForce(transform.forward * (_force / (_angle * 5)), ForceMode.Impulse);
            }


            yield return new WaitForSecondsRealtime(_fireRate);

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