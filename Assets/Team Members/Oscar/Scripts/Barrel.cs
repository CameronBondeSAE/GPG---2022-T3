using UnityEngine;

public class Barrel : MonoBehaviour, IFlammable
{
    public BarrelHealth barrelHealth;

    private float TimeRemaining = 3;

    private bool onFire = false;
    private float myHealth = 100;

    public delegate void OnFire();
    public event OnFire burning;
    public void Start()
    {
        barrelHealth.myAmount(myHealth);
    }

    public void SetOnFire()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
        onFire = true;
        
        print("ow");
    }

    private void Update()
    {
        if (onFire == true)
        {
            TimeRemaining -= Time.deltaTime;
            
            burning?.BeginInvoke(null, null);
            if (TimeRemaining <=0)
            {
                onFire = false;
                TimeRemaining = 5;
            }
        }
        else
        {
            burning?.EndInvoke(null);
        }
    }
}
