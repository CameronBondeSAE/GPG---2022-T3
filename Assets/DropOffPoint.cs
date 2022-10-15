using System.Collections;
using System.Collections.Generic;
using Alex;
using UnityEngine;

public class DropOffPoint : MonoBehaviour
{
    Sensor sensor;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 9)
            {
                sensor.hasResource = false;
            }
        }
}
