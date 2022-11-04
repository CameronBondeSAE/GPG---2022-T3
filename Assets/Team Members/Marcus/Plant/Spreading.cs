using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Spreading : MonoBehaviour, IFlammable
{
    public GameObject seedling;
    public LayerMask layers;
    
    public int spreadLimit;
    private int spreadNumber;

    private Vector3 maxSize;
    private bool matured;

    private float spreadTimer;
    private float spreadDistance;
    private Vector3 spreadDirection;

    private float deaathTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        maxSize = new Vector3(1, 0.05f, 1);
        RandomiseTimer();
    }

    void RandomiseTimer()
    {
        spreadTimer = Random.Range(2f, 5f);
        deaathTimer = Random.Range(4f, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, maxSize, 0.002f);
        
        //if meets a certain size, grow new plants
        if (transform.localScale.magnitude >= maxSize.magnitude)
        {
            spreadTimer -= Time.deltaTime;

            if (spreadTimer <= 0 && spreadNumber < spreadLimit)
            {
                spreadDistance = Random.Range(1f, 3f);
                spreadDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * transform.forward;
                
                Spread(spreadDistance, spreadDirection);
            }
            else if (spreadNumber >= spreadLimit)
            {
                matured = true;
            }
        }

        if (matured)
        {
            deaathTimer -= Time.deltaTime;

            if (deaathTimer <= 0)
            {
                int rEvoChance = Random.Range(0, 10);
                
                if (Physics.OverlapSphere(transform.position, maxSize.x, layers, QueryTriggerInteraction.Collide).Length >=5 && rEvoChance == 1)
                {
                    print("GRR ANGY PLANT");
                    gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void Spread(float distance, Vector3 direction)
    {
        Vector3 pos = transform.position + direction * distance;
        
        //grow new plants
        if (Physics.OverlapSphere(pos, maxSize.x/2, layers, QueryTriggerInteraction.Collide).Length == 0)
        {
            Instantiate(seedling, pos, Quaternion.identity);
        }
        spreadNumber++;
        RandomiseTimer();
    }

    public void SetOnFire()
    {
        //straight die
        //maybe scream or wither
    }
}
