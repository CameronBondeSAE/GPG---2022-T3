using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Lloyd;
using Luke;

public class SpawnManager : NetworkBehaviour
{
    public GameObject bossAI;
    public GameObject swarmerAI;
    
    private GameObject AlienBase()
    {
        GameObject spawnPointObject = null;
        foreach (HQ hq in FindObjectsOfType<HQ>())
        {
            if (hq.type == HQ.HQType.Aliens)
            {
                spawnPointObject = hq.GetComponentInChildren<SpawnPoint>().gameObject;
            }
        }
        return spawnPointObject;
    }

    public void SpawnBossAI()
    {
        for (int j = 0; j < GameManager.singleton.playersInGame; j++)
        {
            GameManager.singleton.NetworkInstantiate(bossAI, AlienBase().GetComponent<SpawnPoint>().transform.position, Quaternion.identity);
        }
    }
    public void SpawnSwarmerAI()
    {
        if (GameManager.singleton.amountOfAIInGame < 100)
        {
            for (int i = 0; i < 5; i++)
            {
                GameManager.singleton.NetworkInstantiate(swarmerAI, AlienBase().GetComponent<SpawnPoint>().transform.position, Quaternion.identity);
                GameManager.singleton.amountOfAIInGame++;
                
            }
        }

    }
}


