using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : MonoBehaviour
{
	public float value;
	public float speed = 1f;
	
	public AnimationCurve Curve1;
	
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    // value += speed * Time.deltaTime;
	    // if(value > 2f) // stop

	    value = Mathf.MoveTowards(value, 2f, speed * Time.deltaTime);
	    
	    transform.localScale = new Vector3(value, value, value);
    }
}
