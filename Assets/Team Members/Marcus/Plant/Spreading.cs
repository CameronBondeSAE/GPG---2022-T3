using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spreading : MonoBehaviour, IFlammable
{
    public GameObject seedling;
    
    public int spreadAmount;
    private int spreadNumber;

    private float maxSize = 1;

    private float spreadTimer;
    private float spreadDistance;
    private Vector3 spreadDirection;
    
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
        else
        {
            print("I can grow");
            spreadTimer = Random.Range(2f, 5f);
            spreadTimer -= Time.deltaTime;

            if (spreadTimer <= 0 && spreadNumber < spreadAmount)
            {
                print("I'm growing now");
                spreadDistance = Random.Range(1, 3);
                spreadDirection = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * transform.forward;
                
                Spread(spreadDistance, spreadDirection);
            }
        }
    }

    void Spread(float distance, Vector3 direction)
    {
        //grow new plants
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, distance);
        
        Instantiate(seedling, hitInfo.point, Quaternion.identity);
        spreadNumber++;
        print("plant growed");
    }

    public void SetOnFire()
    {
        //straight die
        //maybe scream or wither
    }
}
