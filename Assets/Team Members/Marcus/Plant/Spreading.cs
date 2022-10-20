using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spreading : MonoBehaviour, IFlammable
{
    public GameObject seedling;

    private float maxSize = 1;
    private bool canSpread;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if meets a certain size, grow new plants
        if (transform.localScale.x < maxSize)
        {
            transform.localScale += new Vector3(0.1f * Time.deltaTime, 0, 0.1f * Time.deltaTime);
        }
    }

    void Spread()
    {
        //grow new plants
    }

    public void SetOnFire()
    {
        //straight die
        //maybe scream or wither
    }
}
