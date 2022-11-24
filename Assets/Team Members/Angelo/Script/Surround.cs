using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surround : MonoBehaviour
{
    public Transform HomeBase;
    public Transform groundZero;
    public Transform[] SurroundPos;
    public bool[] AvailableSpace;
    public GameObject emptyObject;
    public int amount = 1;

    public int weight;
    private bool carryToBase;
    public float speed;
    public int counter;
    // Start is called before the first frame update
    void Start()
    {
        SurroundPos = new Transform[amount];
        AvailableSpace = new bool[amount];
        float angle = 360f / amount;
        for (int i = 0; i < amount; i++)
        {
            GameObject it = Instantiate(emptyObject,transform);
            
            it.transform.Rotate(Vector3.up, angle * i);
            it.transform.position = transform.position - (it.transform.forward * 2);
            it.AddComponent<CarrySpot>().id = i;
            SurroundPos[i] = it.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWeight();
        
        if (carryToBase)
        {
            Vector3 direction = HomeBase.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * Time.deltaTime * speed);
        }

        counter = 0;
    }

    private void OnDrawGizmos()
    {
        if(SurroundPos.Length == amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (SurroundPos[i] != null)
                {
                    Gizmos.DrawLine(transform.position, SurroundPos[i].position);
                }
            }
        }   
    }

    public Transform Occupy(int set)
    {
        Transform temp;
        if (!AvailableSpace[set])
        {
            AvailableSpace[set] = true;
            temp = SurroundPos[set];
            return temp;
        }
        return SurroundPos[0];
    }

    public void Leave(int set)
    {
        AvailableSpace[set] = false;
    }

    public int SetNumber()
    {
        for (int i = 0; i < amount; i++)
        {
            if (!AvailableSpace[i])
            {
                return i;
            }
        }
        return 0;
    }

    public void UpdateWeight()
    {
        /*foreach (var carry in AvailableSpace)
        {
            if (carry)
            {
                counter += 1;
            }
        }*/

        if (counter >= weight)
        {
            carryToBase = true;
        }
        else
        {
            carryToBase = false;
        }
    }
}
