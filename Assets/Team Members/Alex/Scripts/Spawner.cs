using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Alex
{
    public class Spawner : MonoBehaviour
    {
        public GameObject AlexAI;
        public float spreadAmount;

        public float numberToSpawn = 10;

        // Start is called before the first frame update
        public void Start()
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                Instantiate(AlexAI,
                    transform.position + new Vector3(Random.Range(-spreadAmount, spreadAmount), 0,
                        Random.Range(-spreadAmount, spreadAmount)), Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
        }
    }
}
