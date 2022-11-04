using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BurnVictim_Test : MonoBehaviour, IFlame
{
    [SerializeField] private float _HP;

    [SerializeField] private float _fireMultiplier;

    private float _heatLevel;

    [SerializeField]
    private float _heatMax=10;

    private Renderer _rend;

    private bool _burning=false;
    
    private bool _isAlive=true;

    private void OnEnable()
    {
        _rend = GetComponent<Renderer>();
        _rend.material.SetColor("_BaseColor", Color.green);
        
    }

    public void SetOnFire()
    {
        _burning = true;
        _rend.material.SetColor("_BaseColor", Color.red);
    }

    public void FixedUpdate()
    {
    if(_isAlive){
        if (_burning)
            _HP -= _fireMultiplier;
            }

        if (_HP <= 0)
        {
            _isAlive = false;
            _HP = 0;
            Death();
        }
    }

    private void Death()
    {
     _rend.material.SetColor("_BaseColor", Color.black);

     StartCoroutine(ActualDeath());
    }

    private IEnumerator ActualDeath()
    {
        float rand = Random.Range(3f, 5f);
        
        yield return new WaitForSecondsRealtime(rand);
            Destroy(gameObject);
    }
    
    public void ChangeHeat(float x)
    {
        
        _heatLevel += x;

        if (_heatLevel >= _heatMax) 
            SetOnFire();
    }
}
