using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;
using Unity.Netcode;
using Random = UnityEngine.Random;

public class Flammable : NetworkBehaviour, IHeatSource
{
    //Flammable Component assumes gameObj also has a HealthComponent attached
    private Health _healthComp;

    private FlameModel _flameModel;
    public GameObject flamePrefab;

    //determines how much is inflicted through ChangeHeat
    //does this go here? does flamethrower hold this info? do different objects' fire hurt more? etc
    [Header("Fire Damage")] [SerializeField]
    private float fireDamage;

    [Header("Current Heat Level")] [SerializeField]
    //object's current heat level
    private float heatLevel;

    public float HeatLevel
    {
	    get => heatLevel;
	    set
	    {
		    if (value < 0) heatLevel = 0;
		    else
		    {
			    heatLevel = value;
                if (heatLevel > heatThreshold && !_burning)
                {
                    _burning = true;
                    SetOnFire();
                }
			    if (ticking) return;
			    ticking = true;
			    StartCoroutine(FireTick());
		    }
	    }
    }

    [Header("Object Set On Fire Heat Level")]
    //the threshold at which object is actually alight
    //this is where "On Fire" animations, sounds, logic, etc are activated
    //spawns Flame prefab
    public float heatThreshold;

    //some objects burn faster than others
    //tracked with this float, multiplies ChangeHeat
    [SerializeField] private float heatMultiplier;

    private bool _burning;

    //effectively the fire's HP / lifespan
    //adds current health to use as fuel
    [SerializeField] private float fuel;

    //objects are constantly losing heat every update times coolRate
    [SerializeField] private float coolRate;

    //determines how big the instantiated flame should be
    [SerializeField] private float radius;

    List<GameObject> fireList = new ();

    private bool ticking;

    public event Action CoolDown;

    public override void OnNetworkSpawn()
    {
	    base.OnNetworkSpawn();
	    
	    _healthComp = GetComponent<Health>();

	    fuel += _healthComp.HP;
    }

    private void OnEnable()
    {
	    // CAM: Moved to network start OOO
    }

    private void Start()
    {
	    if (heatLevel > 0)
	    {
		    ticking = true;
		    StartCoroutine(FireTick());
	    }
    }

    private IEnumerator FireTick()
    {
	    // Randomly distributing ticks across frames to prevent lagging.
	    int rand = Random.Range(1, 50);
	    for (int i = 0; i < rand; i++)
	    {
		    yield return new WaitForFixedUpdate();
	    }
	    
	    while (HeatLevel > 0)
	    {
		    yield return new WaitForSeconds(1f); //Prevents this from being called every frame, also means we don't have to account for fixedDeltaTime;
		    if (_burning)
		    {
			    if (_healthComp != null) _healthComp.ChangeHP(-fireDamage); //Burn damage
			    else Debug.Log(gameObject.name);
		    }
		    
		    ChangeHeat(this, -coolRate); //Cooling
		    CoolDown?.Invoke();
	    }

	    ticking = false;
    }

    private void SetOnFire()
    {
	    if (IsServer)
	    {
		    if (fireList.Count > 0)
		    {
			    return;
		    }

		    SetOnFireFunction();
		    HeatLevel += fireDamage;

		    if (flamePrefab == null) return;
		    GameObject fire = Instantiate(flamePrefab, transform.position, Quaternion.identity) as GameObject;
		    fire.transform.SetParent(transform);
		    _flameModel = fire.GetComponent<FlameModel>();
		    SetOnFireClientRPC();
        
		    if (_flameModel == null) return;
		    _flameModel.SetFlameStats(fireDamage, fuel, radius);
		    
		    fireList.Add(fire);
	    }
    }

    [ClientRpc]
    private void SetOnFireClientRPC()
    {
	    if (flamePrefab == null) return;
	    GameObject fire = Instantiate(flamePrefab, transform.position, Quaternion.identity) as GameObject;
	    fire.transform.SetParent(transform);
    }

    public void Extinguish()
    {
	    if (IsServer)
	    {
		    _burning = false;
		    HeatLevel = 0;

		    foreach (GameObject fire in fireList)
		    {
			    _flameModel = fire.GetComponentInChildren<FlameModel>();
			    _flameModel.SetFlameStats(0, 0, 0);
			    Destroy(fire);
		    }
		    ExtinguishClientRPC();
		    fireList.Clear();
	    }
    }

    [ClientRpc]
    private void ExtinguishClientRPC()
    {
	    foreach (GameObject fire in fireList)
	    {
		    Destroy(fire);
	    }
    }
    

    public void ChangeHeat(IHeatSource source, float amount)
    {
        //heatMultiplier is only added for positive change heat input
        //not for water / extinguishers
        if (amount > 0) HeatLevel += amount * heatMultiplier;
        else
        {
	        HeatLevel += amount;
        }
        
        if (HeatLevel > heatThreshold) return;
        if (!(HeatLevel <= 0 || fuel <= 0)) return;
        
        _burning = false;
        HeatLevel = 0;
        // Extinguish();
    }

    public event Action SetOnFireEvent;

    private void SetOnFireFunction()
    {
        SetOnFireEvent?.Invoke();
    }
    
    public void OnCoolDown()
    {
	    CoolDown?.Invoke();
    }
}