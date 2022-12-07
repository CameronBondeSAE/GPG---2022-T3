using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ExplosiveFragments : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        if(NetworkManager.Singleton.IsServer) Destroy(gameObject);
    }
}
