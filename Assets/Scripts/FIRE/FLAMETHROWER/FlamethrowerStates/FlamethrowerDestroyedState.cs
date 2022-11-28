using System;
using System.Collections;
using System.Collections.Generic;
using Lloyd;
using Unity.VisualScripting;
using UnityEngine;

public class FlamethrowerDestroyedState : MonoBehaviour
{
    private FlamethrowerModel model;

    private void OnEnable()
    {
        model = GetComponent<FlamethrowerModel>();
        model.enabled = false;
    }
}
