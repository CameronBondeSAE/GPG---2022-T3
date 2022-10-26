using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingObject : MonoBehaviour
{
    public float pingValue;
    public IEnumerator pinged()
    {
        GetComponent<Renderer>().material.color = Color.green;
        yield return new WaitForSeconds(pingValue);
        GetComponent<Renderer>().material.color = Color.white;
    }
}
