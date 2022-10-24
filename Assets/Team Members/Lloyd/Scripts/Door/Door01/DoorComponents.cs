using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorComponents : MonoBehaviour
{
    public int _doorInt;

    public int GetDoorInt()
    {
        return _doorInt;
    }

    public void SetDoorInt(int x)
    {
        _doorInt = x;
    }

    public float _HP;

    public float GetHP()
    {
        return _HP;
    }

    public void SetHP(float x)
    {
        _HP = x;
    }

    //determines how flammable something is (ie, the higher the float, the faster x will burn)
    public float _fireDamage;

    public float GetFireDamage()
    {
        return _fireDamage;
    }

    public void SetFireDamage(float x)
    {
        _fireDamage = x;
    }

    public float _speed;

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float x)
    {
        _speed = x;
    }
}