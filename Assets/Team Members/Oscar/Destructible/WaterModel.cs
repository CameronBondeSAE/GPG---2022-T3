using System.Collections;
using System.Collections.Generic;
using Lloyd;
using UnityEngine;
using Oscar;
using Unity.Netcode;

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
            if (item.GetComponent<Flammable>() != null)
            {
                flammable = item.GetComponent<Flammable>();
                flammable.ChangeHeat(theHeatSource,-changeHeatAmount);
                flammable.Extinguish();
                flammable.OnCoolDown();
            }

            if (item.GetComponent<FlamethrowerModel>() != null)
            {
                flamethrower = item.GetComponent<FlamethrowerModel>();
                flamethrower.ChangeOverheat(-changeHeatAmount);
            }
        }
        if(NetworkManager.Singleton.IsServer) Destroy(gameObject,2f);
    }
}
