using System;
using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

public class SwarmState : StateBase
{
    public Cohesion cohesion;
    public Align align;
    public Separation separation;
    public Wonder wonder;


    // Start is called before the first frame update
    public void OnEnable()
    {
        cohesion.enabled = true;
        align.enabled = true;
        separation.enabled = true;
        wonder.enabled = true;
    }

    public void OnDisable()
    {
        cohesion.enabled = false;
        align.enabled = false;
        separation.enabled = false;
        wonder.enabled = false;
    }
}
