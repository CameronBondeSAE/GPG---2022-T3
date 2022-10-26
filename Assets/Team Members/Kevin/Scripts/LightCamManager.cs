using System.Collections;
using System.Collections.Generic;
using Kevin;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class LightCamManager : NetworkBehaviour
{
    public GameObject lobbyCamera;
    public GameObject lobbyDirectionalLight;
    
    public void OnStarted()
    {
        lobbyCamera.SetActive(false);
        lobbyDirectionalLight.SetActive(false);
    }
}
