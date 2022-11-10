using Unity.Jobs;
using UnityEngine;

public struct CamJob : IJob
{
	public int number;

	public Vector3 v3;
	
	public float total;
	
    public void Execute()
    {
	    Debug.Log("Start");
	    for (int i = 1; i < number; i++)
	    {
		    // Vector3.Distance(v3, Vector3.one);
		    total += Mathf.Sin(i) * 1.44f / Mathf.Atan(i);
	    }
	    Debug.Log(total);
    }
}
