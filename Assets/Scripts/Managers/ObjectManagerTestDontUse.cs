using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectManagerTestDontUse : NetworkBehaviour
{
    void CreateNewObject(NetworkObject go, Transform pos)
    {
        Instantiate(go);
        go.Spawn();
        go.transform.position = pos.position;
    }

    void DestroyObject(NetworkObject go)
    {
        Destroy(go);
        go.Despawn();
    }
}
