using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour
{
	public int thing;
	public float stuff;
	
	public void Open()
    {
        Debug.Log("Opened door");

        transform.localScale = new Vector3(2, 2, 2);
    }
}
