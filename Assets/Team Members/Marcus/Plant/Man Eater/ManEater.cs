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
    
    private void OnEnable()
    {
        colissionManager.OnTriggerEnterEvent += CheckColliderEntered;
        colissionManager.OnTriggerExitEvent += CheckColliderExited;
        
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
        if (plants.Count < 2)
        {
            Die();
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

    void CheckColliderExited(Collider other)
    {
        if (other.GetComponent<PlantBase>() != null)
        {
            SubtractNeighbours(other);
        }
    }

    void EatThing(Collider player)
    {
        print("Get Nommed");
        //Send damage to player
    }

    void AddNeighbours(Collider plant)
    {
        print("New PLant" + this);
        plants.Add(plant.gameObject);
    }

    void SubtractNeighbours(Collider plant)
    {
        print("Sadge fren ded" + this);
        plants.Remove(plant.gameObject);
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
