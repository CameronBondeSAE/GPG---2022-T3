using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectedByVisibility
{
    public void Detection(float timeOnScreen = 1);
}
