using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;
using Oscar;

public class FlamethrowerShootState : MonoBehaviour, IHeatSource
{
    public ExplosiveIdleState idleState;
    
    public FlamethrowerModel model;

    public GameObject fireball;

    public GameObject waterball;

    public GameObject barrel;

    private Rigidbody rb;

    public Vector3 firePointPos;

    private float force;

    private int altAmmo;

    private float altForce;

    private float fireRate;

    private float altFireRate;

    private bool shooting;

    private bool altShooting;

    private bool waterSpraying;

    private bool isHeld;

    private FlamethrowerModel.FlamethrowerType myType;

    private void OnEnable()
    {
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

        isHeld = model.isHeld;

        Shoot();
    }
    
    /// <summary>
    /// Flamethrower and Water Cannon
    /// </summary>

    public void Shoot()
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
            firePointPos = transform.position + transform.forward * 5;

            Vector3 targetDir = firePointPos - transform.position;

            GameObject _fireball = Instantiate(fireball, transform.position, Quaternion.identity) as GameObject;
            rb = _fireball.GetComponent<Rigidbody>();
            rb.velocity = targetDir * force;
            // rb.AddForce(targetDir * force, ForceMode.Impulse);

            yield return new WaitForSecondsRealtime(fireRate);
        }
    }

    //ALT FIRE GOES HERE
    private IEnumerator AltFire()
    {
        while (altShooting)
        {
            altAmmo = model.altAmmo;
            model.altAmmo--;
            if (altAmmo <= 0)
                break;
            
            
            firePointPos = transform.position + transform.forward * 5;
            
            Vector3 targetDir = firePointPos - transform.position;

            GameObject _barrel = Instantiate(barrel, transform.position, Quaternion.identity) as GameObject;
            // GameObject _fire = Instantiate(fireball, transform.position, Quaternion.identity) as GameObject;
            // _fire.transform.SetParent(_barrel.transform);

            /*idleState = _barrel.GetComponent<ExplosiveIdleState>();
            idleState.SetOnFire();*/
            
            var flammable = _barrel.GetComponent<Flammable>();
            flammable.ChangeHeat(this, 1000f);
            
            rb = _barrel.GetComponent<Rigidbody>();
            rb.velocity = targetDir * model.altForce;
            // rb.AddForce(targetDir * altForce, ForceMode.Impulse);
            
            yield return new WaitForSecondsRealtime(altFireRate);
        }
    }

    private IEnumerator SprayWater()
    {
        while (waterSpraying)
        {
            firePointPos = transform.position + transform.forward * 5;
    
            Vector3 targetDir = firePointPos - transform.position;
    
            GameObject waterPrafab = Instantiate(waterball, transform.position, Quaternion.identity) as GameObject;
            rb = waterPrafab.GetComponent<Rigidbody>();
            rb.AddForce(targetDir * force, ForceMode.Impulse);
    
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
        shooting = false;
        altShooting = false;
        waterSpraying = false;
    }
}