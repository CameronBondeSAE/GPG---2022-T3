using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour, IFlammable
{
    public PHealth health;
    private float myHealth = 100f;

    private bool isAflame;
    private bool hasBurned;
    private float burnDuration;

    private Renderer objColour;

    // Start is called before the first frame update
    void Start()
    {
        health.damagedEvent += FlowerHurty;
        
        objColour = GetComponent<Renderer>();
        objColour.material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAflame)
        {
            burnDuration += Time.deltaTime;
            transform.localScale -= new Vector3(burnDuration, burnDuration/25, burnDuration);

            if (transform.localScale.y < 0)
            {
                isAflame = false;
                hasBurned = true;
            }
        }
        else if (hasBurned)
        {
            objColour.material.color = Color.black;
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
