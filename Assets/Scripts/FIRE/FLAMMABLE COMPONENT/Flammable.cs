using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;

public class Flammable : MonoBehaviour, IHeatSource
{
    //Flammable Component assumes gameObj also has a HealthComponent attached
    private Health healthComp;
    
    public GameObject flameModelPrefab;
    private FlameModel flameModel;
    
    //determines how much is inflicted through ChangeHeat
    //does this go here? does flamethrower hold this info? do different objects' fire hurt more? etc
    [Header("Fire Damage")] [SerializeField]
    private float fireDamage;

    [Header("Current Heat Level")]
    //object's current heat level
    public float heatLevel;

    [Header("Object Set On Fire Heat Level")]
    //the threshold at which object is actually alight
    //this is where "On Fire" animations, sounds, logic, etc are activated
    //spawns Flame prefab
    [SerializeField]
    public float heatThreshold;

    //some objects burn faster than others
    //tracked with this float, multiplies ChangeHeat
    [SerializeField] private float heatMultiplier;

    private bool burning;

    //effectively the fire's HP / lifespan
    //adds current health to use as fuel
    [SerializeField] private float fuel;

    //objects are constantly losing heat every update times coolRate
    [SerializeField] private float coolRate;

    //determines how big the instantiated flame should be
    [SerializeField] private float radius;

    List<GameObject> fireList = new List<GameObject>();

    private void OnEnable()
    {
        healthComp = GetComponent<Health>();

        fuel += healthComp.GetHP();
    }

    public void FixedUpdate()
    {
        if (burning)
            healthComp.ChangeHP(-fireDamage * 0.2f);

        Cool();
    }

    public void SetOnFire()
    {
        if (fireList.Count > 0)
        {
            return;
        }

        OnSetOnFire();
        heatLevel += fireDamage;

        if (flameModelPrefab != null)
        {
            GameObject fire = Instantiate(flameModelPrefab, transform.position, Quaternion.identity) as GameObject;
            fire.transform.SetParent(transform);
            flameModel = fire.GetComponent<FlameModel>();
            if (flameModel != null)
            {
                flameModel.SetFlameStats(fireDamage, fuel, radius);
                

                fireList.Add(fire);
            }
        }
    }

    public void Extinguish()
    {
        burning = false;
        heatLevel = 0;

        foreach (GameObject fire in fireList)
        {
            flameModel = fire.GetComponentInChildren<FlameModel>();
            flameModel.SetFlameStats(0, 0, 0);
            Destroy(fire);
        }
        
        fireList.Clear();
    }


    public void ChangeHeat(IHeatSource x, float y)
    {
        //heatMultiplier is only added for positive change heat input
        //not for water / extinguishers
        if (y > 0)
        {
            heatLevel += y * heatMultiplier;
        }

        else
        {
            heatLevel += y;
        }

        if (heatLevel >= heatThreshold && !burning)
        {
            burning = true;
            SetOnFire();
        }

        if (heatLevel <= heatThreshold || fuel <= 0)
        {
            burning = false;
        }

        if (heatLevel <= 0)
        {
            heatLevel = 0;
           // Extinguish();
        }
    }

    public event Action CoolDown;
    public void OnCoolDown()
    {
        CoolDown?.Invoke();
    }
    
    private void Cool()
    {
        ChangeHeat(this, -coolRate * Time.deltaTime);
    }

    public event Action SetOnFireEvent;

    public void OnSetOnFire()
    {
        SetOnFireEvent?.Invoke();
    }
}