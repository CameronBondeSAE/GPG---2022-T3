using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    private GameObject myself;

    void OnEnable()
    {
        myself = this.GameObject();

        Lloyd.EventManager.TerrainClearEvent += KillSelf;
    }

    void OnDisable()
    {
        Lloyd.EventManager.TerrainClearEvent -= KillSelf;
    }

    public void KillSelf()
    {
        Destroy(myself);
    }
}