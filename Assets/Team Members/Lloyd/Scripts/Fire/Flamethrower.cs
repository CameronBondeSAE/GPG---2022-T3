using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lloyd
{

public class Flamethrower : MonoBehaviour
{
public Transform _firePoint;

private Rigidbody _firerb;

[SerializeField] private float _force;

public GameObject _fireball;

[SerializeField] private float _fireRate;

private void OnEnable()
{
    StartCoroutine(Shoot());
}

private IEnumerator Shoot()
{
    ShootFire();
    
    yield return new WaitForSeconds(_fireRate);

    StartCoroutine(Shoot());
}


private void ShootFire()
{
    GameObject _fire = Instantiate(_fireball, this.transform.forward, Quaternion.identity) as GameObject;
    _firerb = _fire.GetComponent<Rigidbody>();
    _firerb.AddForce(transform.forward * _force,ForceMode.Impulse);
    
    //Destroy(_firerb);

}



}

}