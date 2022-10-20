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
    
    public delegate void ChangeHealth(float amount);

    public static event ChangeHealth ChangeHealthEvent;

    public static void ChangeHealthFunction(float amount)
    {
        if (ChangeHealthEvent != null)
        {
            ChangeHealthEvent(amount);
        }
    }

}
