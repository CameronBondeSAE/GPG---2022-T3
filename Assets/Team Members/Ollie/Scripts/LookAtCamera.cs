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
    //TODO: Ollie
    //look at making this a clientRpc somehow - clients will see other players' score rotating to the other player's camera
    //rather than them ALL looking at client's camera
    private void LateUpdate()
    {
        transform.LookAt(GameManager.singleton.cameraBrain.OutputCamera.transform);
        transform.Rotate(0,180,0);
    }
}

#endregion
