using System;
using System.Collections;
using System.Collections.Generic;
using Marcus;
using UnityEngine;

public class ManEater : MonoBehaviour, IFlammable
{
    public ColissionManager colissionManager;
    public Flammable fireness;

    private List<GameObject> plants;
    private float checkTimer = 1f;
    
    private void OnEnable()
    {
        colissionManager.OnTriggerEnterEvent += CheckColliderEntered;
        fireness.SetOnFireEvent += SetOnFire;
    }

    // Start is called before the first frame update
    void Start()
    {
        plants = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0)
        {
            CheckListForNull();
        }
        
        if (plants.Count <= 2)
        {
            Die();
        }
    }

    void CheckListForNull()
    {
        foreach (GameObject item in plants)
        {
            if (item == null)
            {
                plants.Remove(item);
            }
        }
    }

    void CheckColliderEntered(Collider other)
    {
        if (other.GetComponent<IControllable>() != null)
        {
            EatThing(other);
        }

        if (other.GetComponent<PlantBase>() != null)
        {
            AddNeighbours(other);
        }
    }

    void EatThing(Collider player)
    {
        print("Get Nommed");
        //Send damage to player
    }

    void AddNeighbours(Collider plant)
    {
        plants.Add(plant.gameObject);
    }

    public void SetOnFire()
    {
        //Well die but more dramatically
        //Maybe spread some man eater virus to nearby plants
        print("GGGRRR ANGRY DEATH NOISES!!!");
        Die();
    }

    public void ChangeHeat(IHeatSource heatSource, float x)
    {
        
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
