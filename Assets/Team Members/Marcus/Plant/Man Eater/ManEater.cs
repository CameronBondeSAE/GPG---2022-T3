using System;
using System.Collections;
using System.Collections.Generic;
using Marcus;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

public class ManEater : MonoBehaviour, IFlammable
{
    public ColissionManager colissionManager;
    public Flammable fireness;
    public Health health;

    public int damage;

    [SerializeField] private List<GameObject> plants;
    private float checkTimer = 1f;
    private float deathTimer = 3f;
    
    private void OnEnable()
    {
        colissionManager.OnTriggerEnterEvent += CheckColliderEntered;
        fireness.SetOnFireEvent += SetOnFire;
        health.YouDied += Die;
    }

    // Start is called before the first frame update
    void Start()
    {
        plants = new List<GameObject>();
        StartCoroutine(CheckTimer());
    }

    public delegate void ManeaterDeath();
    public event ManeaterDeath maneaterDeathEvent;

    public delegate void ManeaterBurning();
    public event ManeaterBurning maneaterBurnEvent;

    void CheckListForNull()
    {
        for (int i = 0; i < plants.Count; i++)
        {
            if (plants[i] == null)
            {
                plants.Remove(plants[i]);
            }
        }
        
        if (plants.Count <= 2)
        {
            health.ChangeHP(-1000000);
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
        if(player.GetComponent<Health>() != null) player.GetComponent<Health>().ChangeHP(damage);
    }

    void AddNeighbours(Collider plant)
    {
        plants.Add(plant.gameObject);
    }

    public void SetOnFire()
    {
        maneaterBurnEvent?.Invoke();
    }

    public void ChangeHeat(IHeatSource heatSource, float x)
    {
        
    }

    void Die(GameObject go)
    {
        maneaterDeathEvent?.Invoke();
        StartCoroutine(DeathTimer());
    }

    IEnumerator CheckTimer()
    {
        yield return new WaitForSeconds(checkTimer);
        CheckListForNull();
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(deathTimer);
        Destroy(gameObject);
        //SERVER ONLY
    }
}
