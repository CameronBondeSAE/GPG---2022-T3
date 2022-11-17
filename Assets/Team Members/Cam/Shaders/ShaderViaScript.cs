using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderViaScript : MonoBehaviour
{
	public Material mat;
	
    // Update is called once per frame
    void Update()
    {
        mat.SetFloat("_Pulse_intensity", Mathf.PerlinNoise(Time.time,0)*100f);
    }
}
