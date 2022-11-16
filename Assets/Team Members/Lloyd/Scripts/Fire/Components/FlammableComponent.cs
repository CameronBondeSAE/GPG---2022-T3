using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;

public class FlammableComponent : MonoBehaviour
{
    //Flammable Component assumes gameObj also has a HealthComponent attached
    private HealthComponent healthComp;

    private FlameModel flameModel;
    public GameObject flamePrefab;
    
    //determines how much is inflicted through ChangeHeat
    //does this go here? does flamethrower hold this info? do different objects' fire hurt more? etc
    [Header ("Fire Damage")]
    [SerializeField] private float fireDamage;
    
    [Header("Current Heat Level")]
    //object's current heat level
    public float heatLevel;
    
    [Header ("Object Set On Fire Heat Level")]
    //the threshold at which object is actually alight
    //this is where "On Fire" animations, sounds, logic, etc are activated
    //spawns Flame prefab
    [SerializeField] private float heatThreshold;
    
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
        healthComp = GetComponent<HealthComponent>();
        
        fuel += healthComp.GetHP();
    }    
    
    public void FixedUpdate()
         {
             if(burning)
                 healthComp.ChangeHP(-fireDamage*0.2f);
             
             Cool();
         }

    public void SetOnFire()
    {
        if (fireList.Count > 0)
        {
            return;
        }
        SetOnFireFunction();
        heatLevel += fireDamage;

        if (flamePrefab != null)
        {
            GameObject fire = Instantiate(flamePrefab, transform.position, Quaternion.identity) as GameObject;
            fire.transform.SetParent(transform);
            flameModel = fire.GetComponent<FlameModel>();
            flameModel.SetFlameStats(fireDamage, fuel, radius);
            
            fireList.Add(fire);
        }
    }

    public void Extinguish()
    {
        burning = false;
        
        foreach (GameObject fire in fireList){
                flameModel = fire.GetComponent<FlameModel>();
                flameModel.SetFlameStats(0, 0, 0);
                fireList.Clear();
        }
    }


    public void ChangeHeat(float x)
    {
        //heatMultiplier is only added for positive change heat input
        //not for water / extinguishers
        if (x > 0)
        {
            heatLevel += x * heatMultiplier;
        }

        else
        {
            heatLevel += x;
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
            Extinguish();
        }
    }
    
    public event Action SetOnFireEvent;

    private void SetOnFireFunction()
    {
        SetOnFireEvent?.Invoke();
    }

    private void Cool()
    {
        ChangeHeat(-coolRate*.2f);
    }
}
