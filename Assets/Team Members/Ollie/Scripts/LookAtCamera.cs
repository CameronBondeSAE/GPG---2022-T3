using System;
using System.Collections;
using System.Collections.Generic;
using Kevin;
using Unity.Netcode;
using UnityEngine;
using GameManager = Luke.GameManager;

#region Ollie

public class LookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(GameManager.singleton.cameraBrain.OutputCamera.transform);
        transform.Rotate(0,180,0);
    }
}

#endregion
