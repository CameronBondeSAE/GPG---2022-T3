using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingComponent : MonoBehaviour
{
    public SoundEmitter lastHeard;
    

    public void HeardSomething(SoundEmitter thingThatEmittedSound, float radius)
    {
        lastHeard = thingThatEmittedSound;
    }
}
