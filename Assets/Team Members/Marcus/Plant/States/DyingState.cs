using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

namespace Marcus
{
    public class DyingState : MonoBehaviour
    {
        //Change colour to brown and do other dying things
        private void Start()
        {
            Destroy(gameObject);
        }
    }
}
