using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lloyd
{

public class Flamethrower : MonoBehaviour
{
public Transform _firePoint;

private Rigidbody _firerb;

private float _force;

public GameObject _fireball;
        

private void ShootFire()
{
    GameObject _fire = Instantiate(_fireball, this.transform.position, Quaternion.identity) as GameObject;
    _firerb = _fire.GetComponent<Rigidbody>();
    _firerb.AddForce(transform.forward * _force,ForceMode.Impulse);

    //    Destroy(_firerb);

}



}

}