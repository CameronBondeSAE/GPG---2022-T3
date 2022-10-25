using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorComponents : MonoBehaviour
{
    public int _mainDoorInt;
    
    public int _doorWingInt;

   public float _HP;

   //determines how flammable something is (ie, the higher the float, the faster x will burn)
    public float _fireDamage;

   public float _speed;

   public void SetDoorComps(int a, int b, float c, float d, float e)
    {
        _mainDoorInt = a;

        _doorWingInt = b;

        _HP = c;

        _fireDamage = d;

        _speed = e;
    } 
    
    public void GetDoorComps(out int a, out int b,out float c,out float d,out float e)
    {
        a = _mainDoorInt;

        b = _doorWingInt;

        c = _HP;

        d = _fireDamage;

        e = _speed;
    }

}