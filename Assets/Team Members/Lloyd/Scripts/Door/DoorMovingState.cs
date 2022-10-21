using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovingState : MonoBehaviour
{
    private bool isOpen=false;

    private Vector2 moveDir;

    public float moveDist;

    public float speed;

    public void IsOpen()
    {
        isOpen = !isOpen;
    }

    private void Move()
    {
        if (isOpen)
        {
            moveDir = Vector2.down;
            
        }

    }

    private void Update()
    {
          
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
