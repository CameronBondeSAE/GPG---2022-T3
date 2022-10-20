using UnityEngine;

public class BarrelHealth : MonoBehaviour
{
    //public Explosive_Model explosiveModel;
    float amount;

    private float damage = 0.1f;
    
    public void OnEnable()
    {
        //explosiveModel.burning += Damage;
    }

    public float myAmount(float myHealth)
    {
        return amount;
    }
    
    public void Damage()
    {
        amount-=damage;
        print("ow!" + amount + " health left");
    }
}
