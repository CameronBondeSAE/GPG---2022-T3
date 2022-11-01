using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Spreading : MonoBehaviour, IFlammable
{
    public GameObject seedling;
    
    public int spreadLimit;
    private int spreadNumber;

    private float maxSize = 1;

    private float spreadTimer;
    private float spreadDistance;
    private Vector3 spreadDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        RandomiseTimer();
    }

    void RandomiseTimer()
    {
        spreadTimer = Random.Range(2f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //if meets a certain size, grow new plants
        if (transform.localScale.x < maxSize)
        {
            transform.localScale += new Vector3(0.1f * Time.deltaTime, 0, 0.1f * Time.deltaTime);
        }
        else
        {
            spreadTimer -= Time.deltaTime;

            if (spreadTimer <= 0 && spreadNumber < spreadLimit)
            {
                spreadDistance = Random.Range(1, 3);
                spreadDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * transform.forward;
                
                Spread(spreadDistance, spreadDirection);
            }
        }
    }

    void Spread(float distance, Vector3 direction)
    {
        Vector3 pos = transform.position + direction;
        
        //grow new plants
        foreach (Collider item in Physics.OverlapSphere(pos, distance))
        {
            if (item /*is an empty space*/)
            {
                Instantiate(seedling, pos, Quaternion.identity);
                spreadNumber++;
                RandomiseTimer();
            }
        }
    }

    public void SetOnFire()
    {
        //straight die
        //maybe scream or wither
    }
}
