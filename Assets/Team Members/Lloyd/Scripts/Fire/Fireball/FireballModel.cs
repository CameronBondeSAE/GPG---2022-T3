using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballModel : MonoBehaviour
{
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

    private float _distance;
    
    private Vector3 _center;

    private Vector3 _burnVictim;

    private Renderer _rend;

    private void OnEnable()
    {
        _rend = GetComponent<Renderer>();
        
        _rend.material.SetColor("_BaseColor", new Color(1f,0,0,.5f));
    }

    private void FixedUpdate()
    {
        _center = this.transform.position;
                
        //would it be more efficient to run two overlap spheres or calculate dist with one sphere?
        //
                
                
        Collider[] hitColliders = Physics.OverlapSphere(_center, _radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<IFlame>() != null)
            {
                hitCollider.GetComponent<IFlame>().ChangeHeat(_heat);

                _burnVictim = hitCollider.transform.position;
                
                _distance = Vector3.Distance(_center, _burnVictim);
                if (_distance > _minDistance)
                {
                    hitCollider.GetComponent<IFlame>().ChangeHeat(_heat*_proximityMultiplier);
                }
            }
        }

        _lifespan--;
        if (_lifespan <= 0)
            Destroy(this);
    }

    private void OnDisable()
    {
        //death animation
    }
    
}