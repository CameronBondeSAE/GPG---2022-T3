using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectedByVisibilty
{
    public void Detection(float timeOnScreen = 1);
}
