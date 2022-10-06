using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void TerrainClear();

    public static event TerrainClear TerrainClearEvent;

    public static void TerrainClearFunction()
    {
        if (TerrainClearEvent != null)
        {
            TerrainClearEvent();
        }
    }



}
