using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;
using Luke;
using Oscar;
using Unity.Netcode;

public class FlamethrowerShootState : MonoBehaviour, IHeatSource
{
    public FlamethrowerModel model;

    public GameObject fireball;

    public GameObject waterball;

    public GameObject barrel;

    private Rigidbody rb;

    public Vector3 firePointPos;

    private float force;

    private float altForce;

    private float fireRate;

    private float altFireRate;

    private bool shooting;

    private bool altShooting;

    private bool waterSpraying;

    private bool isHeld;

    private int barrelLength;

    private FlamethrowerModel.FlamethrowerType myType;

    private NetworkManager _nm;

    private void OnEnable()
    {
	    _nm = NetworkManager.Singleton;
	    if (!_nm.IsServer) return;
        model = GetComponent<FlamethrowerModel>();

        fireball = model.fireball;
        waterball = model.waterball;
        barrel = model.barrel;

        force = model.force;
        altForce = model.altForce;

        fireRate = model.fireRate;
        altFireRate = model.altFireRate;

        shooting = model.shooting;
        altShooting = model.altShooting;
        waterSpraying = model.waterSpraying;

        barrelLength = model.barrelLength;

        isHeld = model.isHeld;

        Shoot();
    }
    
    /// <summary>
    /// Flamethrower and Water Cannon
    /// </summary>

    private void Shoot()
    {
        if (isHeld && shooting)
            StartCoroutine(SpitFire());

        else if (altShooting)
            StartCoroutine(AltFire());
        
        else if (waterSpraying)
        {
            StartCoroutine(SprayWater());
        }

        else if (!isHeld)
            ShootTilDead();
    }

    private IEnumerator SpitFire()
    {
        while (shooting)
        {
	        Transform t = transform;
	        Vector3 position = t.position;
	        Vector3 forward = t.forward;
            firePointPos = position + forward * barrelLength;

            GameObject _fireball = GameManager.singleton.NetworkInstantiate(fireball, position, t.rotation);
            rb = _fireball.GetComponent<Rigidbody>();
            rb.velocity = forward * force;
            // rb.AddForce(targetDir * force, ForceMode.Impulse);

            yield return new WaitForSeconds(fireRate);
        }
    }

    //ALT FIRE GOES HERE
    private IEnumerator AltFire()
    {
        while (altShooting)
        {
            model.altAmmo--;
            if (model.altAmmo <= 0)
                break;

            Transform t = transform;
            Vector3 position = t.position;
            Vector3 forward = t.forward;
            firePointPos = position + forward * barrelLength;

            GameObject _barrel = GameManager.singleton.NetworkInstantiate(barrel, position, t.rotation);
            // GameObject _fire = Instantiate(fireball, transform.position, Quaternion.identity) as GameObject;
            // _fire.transform.SetParent(_barrel.transform);

            /*idleState = _barrel.GetComponent<ExplosiveIdleState>();
            idleState.SetOnFire();*/
            
            Flammable flammable = _barrel.GetComponent<Flammable>();
            flammable.ChangeHeat(this, 1000f);
            
            rb = _barrel.GetComponent<Rigidbody>();
            rb.velocity = forward * model.altForce;
            // rb.AddForce(targetDir * altForce, ForceMode.Impulse);
            
            yield return new WaitForSeconds(altFireRate);
        }
    }

    private IEnumerator SprayWater()
    {
        while (waterSpraying)
        {
	        Transform t = transform;
	        Vector3 position = t.position;
	        Vector3 forward = t.forward;
	        firePointPos = position + forward * barrelLength;

	        GameObject waterPrefab = GameManager.singleton.NetworkInstantiate(waterball, position, t.rotation);
            rb = waterPrefab.GetComponent<Rigidbody>();
            rb.AddForce(forward * force, ForceMode.Impulse);
    
            yield return new WaitForSeconds(fireRate);
        }
        
    }

    private void ShootTilDead()
    {
        shooting = true;
        StartCoroutine(SpitFire());
    }

    private void OnDisable()
    {
	    if (!_nm.IsServer) return;
        shooting = false;
        altShooting = false;
        waterSpraying = false;
    }
}