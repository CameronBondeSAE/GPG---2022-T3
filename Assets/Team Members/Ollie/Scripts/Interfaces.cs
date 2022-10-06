using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    void PickedUp(Transform parentTransform);
    void PutDown();
}
