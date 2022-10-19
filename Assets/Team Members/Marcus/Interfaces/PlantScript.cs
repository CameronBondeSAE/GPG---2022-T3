using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour, IFlammable
{
    public PHealth health;
    private float myHealth = 100f;

    private bool isAflame = false;
    private bool hasBurned = false;
    private float burnDuration;

    // Start is called before the first frame update
    void Start()
    {
        health.damagedEvent += FlowerHurty;
        this.GetComponent<Renderer>().material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAflame)
        {
            burnDuration += Time.deltaTime;
            this.transform.localScale -= new Vector3(burnDuration, burnDuration/25, burnDuration);

            if (this.transform.localScale.y < 0)
            {
                isAflame = false;
                hasBurned = true;
            }
        }
        else if (hasBurned)
        {
            this.GetComponent<Renderer>().material.color = Color.black;
        }
    }

    void FlowerHurty(float damage)
    {
        myHealth -= damage;
        print("Ouchies");
    }

    public void SetOnFire()
    {
        isAflame = true;
    }
}
