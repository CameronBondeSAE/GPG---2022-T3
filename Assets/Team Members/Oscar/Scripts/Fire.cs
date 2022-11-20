using Oscar;
using UnityEngine;

public class Fire : MonoBehaviour, IHeatSource
{
    //private Explosive_Model _explosiveModelHurt;
    private IHeatSource theheat;
    private void OnTriggerEnter(Collider other)
    {
        //if the collision is with something that has IFlammable on it
        //set it on fire!
        if (other.GetComponent<Flammable>() != null)
        {
            other.GetComponent<Flammable>().ChangeHeat(25);
        }
    }
}
