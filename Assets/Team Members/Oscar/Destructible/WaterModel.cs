using System.Collections;
using System.Collections.Generic;
using Lloyd;
using UnityEngine;
using Oscar;

public class WaterModel : MonoBehaviour
{
    public float radius;

    private Flammable flammable;

    private FlamethrowerModel flamethrower;
    
    private IHeatSource theHeatSource;

    [SerializeField] private float changeHeatAmount;
    
    void FixedUpdate()
    {
        Collider[] splashColliders = Physics.OverlapSphere(transform.position, radius, 9999999, QueryTriggerInteraction.Collide);
        foreach (Collider item in splashColliders)
        {
            if (item.GetComponents<Flammable>() != null)
            {
                flammable = item.GetComponent<Flammable>();
                flammable.ChangeHeat(theHeatSource,-changeHeatAmount);
                flammable.Extinguish();
                flammable.OnCoolDown();
                
            }

            if (item.GetComponents<FlamethrowerModel>() != null)
            {
                flamethrower = item.GetComponent<FlamethrowerModel>();
                flamethrower.ChangeOverheat(changeHeatAmount);
            }
        }
    }
}
