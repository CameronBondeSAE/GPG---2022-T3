using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class FireballModel : NetworkBehaviour
{
    //this is what the flamethrower shoots
    
    //firePrefab
    //simple sprite
    public GameObject _fire01Prefab;
    
    //fireball's firepower
    [SerializeField] private float _heat;

    //fireball's max size
    [SerializeField] private float _radius;

    //heat is hotter the closer it is to center
    [SerializeField] private float _proximityMultiplier;

    //minimum distance between ball center and IFlammable to just set on fire? / increase firepower by lots
    [SerializeField] private float _minDistance;

    //how long the fireball remains active
    [SerializeField] private float _lifespan;
    
    private bool _isActive=true;

    private float _distance;

    private Vector3 _center;

    private Vector3 _burnVictim;

    private Renderer _rend;

    public FireballView _fireballView;

    private Rigidbody _rb;

    private IHeatSource theHeatSource;
    
    [SerializeField]private int maxRoundRobin;
    private float roundRobin;

    private float randomRobin;

    public void SetStats(float heat, float radius, float lifespan)
    {
        _heat = heat;
        _radius = radius;
        _lifespan = lifespan;
    }

    public override void OnNetworkSpawn()
    {
	    base.OnNetworkSpawn();
	    
	    if(IsServer) StartCoroutine(TickTock());
    }

    private void OnEnable()
    {
        _rend = GetComponent<Renderer>();

        _rend.material.SetColor("_BaseColor", new Color(1f, 0, 0, .5f));

        Physics.IgnoreLayerCollision(0,9);
        Physics.IgnoreLayerCollision(9, 9);
        
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
	    if(!IsServer) return;
	    roundRobin++;
	    if (roundRobin <= maxRoundRobin)
	    {
		    CastFire();
		    roundRobin = 0;
	    }
    }

    private void CastFire()
    {
	    Transform t = transform;
        _center = t.position;

        //would it be more efficient to run two overlap spheres or calculate dist with one sphere?
        //
        
        Collider[] hitColliders = Physics.OverlapSphere(_center, _radius, 9999999, QueryTriggerInteraction.Collide);
        foreach (var hitCollider in hitColliders)
        {
            //GameObject fire = Instantiate(_fire01Prefab, transform.position, Quaternion.identity) as GameObject;
            
            if (hitCollider.GetComponent<Flammable>() != null)
            {
                hitCollider.GetComponent<Flammable>().ChangeHeat(theHeatSource,_heat);

                _burnVictim = hitCollider.transform.position;

                _distance = Vector3.Distance(_center, _burnVictim);
                if (_distance > _minDistance)
                {
                    hitCollider.GetComponent<Flammable>().ChangeHeat(theHeatSource,_heat * _proximityMultiplier);
                }
                //StartCoroutine(Death());
                /*Destroy(gameObject);
                transform.SetParent(hitCollider.transform);
                _rb.isKinematic = true;*/
            } 
        }
    }
    
    private IEnumerator TickTock()
    {
        yield return new WaitForSeconds(_lifespan);
        if(_isActive) StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        _isActive = false;
        _fireballView.Death();

        //float rand = Random.Range(0.1f, 2.2f);

        yield return new WaitForSeconds(1);

        SpawnFireClientRpc();

        Destroy(gameObject);
    }

    [ClientRpc]
    void SpawnFireClientRpc()
    {
	    Transform t = transform;
	    Instantiate(_fire01Prefab, t.position, t.rotation);
    }

    private void OnDisable()
    {
        //death animation
    }
}