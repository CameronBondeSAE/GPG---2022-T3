using System.Collections;
using System.Collections.Generic;
using Marcus;
using UnityEngine;

public class PlantView : MonoBehaviour
{
    public GrowthState plantModel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(plantModel.age, 0.05f, plantModel.age);
    }
}
