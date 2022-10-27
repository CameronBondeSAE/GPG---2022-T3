using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingObject : MonoBehaviour
{
    private float pingValue = 1.5f;
    public IEnumerator pinged()
    {
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(pingValue);
        GetComponent<Renderer>().material.color = Color.white;
    }
}
