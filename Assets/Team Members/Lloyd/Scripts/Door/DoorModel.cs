using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Conditions;
using Unity.VisualScripting;
using UnityEngine;

namespace Lloyd
{

    public class DoorModel : MonoBehaviour, IFlammable, IInteractable
{
    //determines if door can be Interacted with
    private bool isActive = true;
   
    //Door Health
    [SerializeField] private float _HP; 
    private DoorComponents HPComp01; 
    private DoorComponents HPComp02;

    //Door Fire Damge
    [SerializeField] private float _fireDamage;
    
    private bool isBurning;

    //Door Movement
    Vector3 doorPos;
    private bool isOpen=false;

    [SerializeField] private float _speed;
    
    //Door Wing GameObjects
    private float _doorWingSize;

    public GameObject _doorWingprefab;
    private GameObject _doorWing01;
    private GameObject _doorWing02;

    private Renderer _doorRend01;
    private Renderer _doorRend02;

    private Vector3 _doorWingPos;

    void OnEnable()
    {
        Lloyd.EventManager.ChangeHealthEvent += ChangeHP;
        Lloyd.EventManager.DoorOpenEvent += FlipActive;
        Lloyd.EventManager.DoorCloseEvent += FlipActive;

        doorPos = this.transform.position;

        SpawnDoors();
    }

    private void SpawnDoors()
    {
        _doorWingPos = new Vector3(transform.position.x - (_doorWingSize), transform.position.y, transform.position.z);

        _doorWing01 = Instantiate(_doorWingprefab, _doorWingPos, Quaternion.identity) as GameObject;

        HPComp01 = _doorWing01.GetComponent<DoorComponents>();
        HPComp01.SetHP(_HP);
        HPComp01.SetFireDamage(_fireDamage);
        HPComp01.SetSpeed(_speed);
        //Instantiate(_doorWingprefab, _doorWingPos, Quaternion.identity); 
        
        
        
        HPComp01 = _doorWing01.GetComponent<DoorComponents>();
        
        _doorWingPos = new Vector3(transform.position.x + (_doorWingSize), transform.position.y, transform.position.z);
        
        _doorWing02 = Instantiate(_doorWingprefab, _doorWingPos, Quaternion.identity) as GameObject;

       // Instantiate(_doorWingprefab, _doorWingPos, Quaternion.identity); 
        
        HPComp02 = GetComponent<DoorComponents>();
        HPComp02.SetHP(_HP);
        HPComp02.SetFireDamage(_fireDamage);
        HPComp02.SetSpeed(_speed);
    }
    
    

    public void Interact()
    {
        if (isActive)
        {
            isOpen = !isOpen;
            if (isOpen)
                Lloyd.EventManager.DoorOpenEventFunction();

            else if (!isOpen)
                Lloyd.EventManager.DoorCloseEventFunction();

            isActive = false;
        }
    }

    private void FlipActive()
    {
        isActive = !isActive;
    }

    private void FixedUpdate()
    {
        if (isBurning)
        {
            EventManager.ChangeHealthFunction(-_fireDamage);
        }
    }


    public void SetOnFire()
    {
        Lloyd.EventManager.BurningEventFunction();
        isBurning = true;
    }

    public void Burnt()
    {
        Lloyd.EventManager.BurntEventFunction();
        _HP = 0;
    }

   

    private void OnDisable()
    {
        Lloyd.EventManager.ChangeHealthEvent -= ChangeHP;
        Lloyd.EventManager.DoorOpenEvent -= FlipActive;
        Lloyd.EventManager.DoorCloseEvent -= FlipActive;
    }

    private void ChangeHP(float amount)
    {
        _HP += amount;
        
        if (_HP <= 0)
            Burnt();
    }

    public float GetSpeed()
    {
        return _speed;
    }
    
}
}