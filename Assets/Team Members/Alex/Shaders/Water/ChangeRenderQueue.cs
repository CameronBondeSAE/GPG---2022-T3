using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRenderQueue : MonoBehaviour
{
    public int renderQueuePosition = -1;

    void Start(){
        GetComponent<Renderer>().material.renderQueue = renderQueuePosition;
    }
}
