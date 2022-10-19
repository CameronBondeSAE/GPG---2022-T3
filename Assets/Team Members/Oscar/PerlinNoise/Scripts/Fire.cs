using UnityEngine;

public class Fire : MonoBehaviour
{
    private Barrel _barrelHurt;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BarrelHealth>())
        {
            _barrelHurt = other.GetComponent<Barrel>();
            _barrelHurt.SetOnFire();
        }
    }
}
