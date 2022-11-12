using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingObject : MonoBehaviour
{
    private float pingValue = 1.5f;

    public void pinged()
    {
        StartCoroutine(changeColor());
    }
    
    public IEnumerator changeColor()
    {
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(pingValue);
        GetComponent<Renderer>().material.color = Color.white;
    }
}
