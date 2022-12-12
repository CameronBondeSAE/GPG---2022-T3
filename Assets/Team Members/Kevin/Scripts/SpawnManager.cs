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
    public GameObject flamethrowerPrefab;
    public GameObject waterCannonPrefab;
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
	    GameObject alienBase = AlienBase();
	    if (alienBase == null) return;
        for (int j = 0; j < GameManager.singleton.playersInGame; j++)
        {
            GameManager.singleton.NetworkInstantiate(bossAI, GameManager.singleton.hqSpawnPointObject[j].transform.position, Quaternion.identity);
        }
    }
    public void SpawnSwarmerAI()
    {
        GameObject alienBase = AlienBase();
        if (alienBase == null) return;
        if (GameManager.singleton.amountOfAIInGame < 100)
        {
            for (int i = 0; i < GameManager.singleton.hqSpawnPointObject.Count; i++)
            {
                GameManager.singleton.NetworkInstantiate(swarmerAI, GameManager.singleton.hqSpawnPointObject[i].transform.position, Quaternion.identity);
                GameManager.singleton.amountOfAIInGame++;
            }
        }

    }

    public void SpawnFlameThrowers()
    {
        for (int i = 0; i < GameManager.singleton.flamethrowerSpawnPointObject.Count; i++)
        {
            GameManager.singleton.NetworkInstantiate(flamethrowerPrefab, GameManager.singleton.flamethrowerSpawnPointObject[i].transform.position, 
                Quaternion.Euler(0,-90,0));
        }
    }

    public void SpawnWaterCannon()
    {
        for (int i = 0; i < GameManager.singleton.waterCannonSpawnPointObject.Count; i++)
        {
            GameManager.singleton.NetworkInstantiate(waterCannonPrefab,
                GameManager.singleton.waterCannonSpawnPointObject[i].transform.position, Quaternion.identity);
        }
    }
}


