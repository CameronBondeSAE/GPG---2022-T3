using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanningRadar : MonoBehaviour, IInteractable, IPickupable
{
    public int rays;
    private float raySpacing;

    private int cycles;
    private bool scanning;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        raySpacing = 20f / rays;

        if (cycles < 360 - rays && scanning)
        {
            for (int i = 0; i < rays; i ++)
            {
                Vector3 scanDir = Quaternion.Euler(0,  i * raySpacing + cycles, 0) * transform.forward;
                Ray radar01 = new Ray(transform.position, scanDir);
                RaycastHit Hit01;
                Physics.Raycast(radar01, out Hit01);
                
                // Just some silly reflection stuff because I wanted to test it out
                /*Vector3 reflect = Vector3.Reflect(radar01.direction, Hit01.normal);
                RaycastHit hitReflection;
                Physics.Raycast(Hit01.point, reflect, out hitReflection);

                Vector3 secondaryRef = Vector3.Reflect(reflect, hitReflection.normal);
                RaycastHit hitSecondary;
                Physics.Raycast(hitReflection.point, secondaryRef, out hitSecondary);*/
            }
            cycles++;
        }
        else if (cycles == 360 - rays && scanning)
        {
            StartScan();
        }
    }

    public void StartScan()
    {
        cycles = 0;
        scanning = !scanning;
    }

    #region Interface Functions
    public void Interact(GameObject Interactor)
    {
        StartScan();
    }

    public void CancelInteract()
    {
        
    }

    public void AltInteract(GameObject interactor)
    {
        //idf nothing. maybe pulse???
        //I dont know why i'm bothering This is obsolete
    }

    public void CancelAltInteract()
    {

    }

    public void PickedUp(GameObject Interactor)
    {
        //Make a bwep bwep sound
    }

    public void PutDown(GameObject Interactor)
    {
        //Make a beowp sound
    }

    public void DestroySelf()
    {
        
    }

    public bool isHeld { get; set; }
    public bool locked { get; set; }
    public bool autoPickup { get; set; }

    #endregion
}
