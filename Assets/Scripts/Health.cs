   using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lloyd;
using Sirenix.OdinInspector;
   using Unity.Netcode;

   public class Health : NetworkBehaviour
{
    [SerializeField] private float maxHP;
    private NetworkVariable<float> hp = new NetworkVariable<float>();
    public float HP
    {
	    get { return hp.Value; }
    }

    public bool isAlive=true;

    public override void OnNetworkSpawn()
    {
	    base.OnNetworkSpawn();
	    
	    OnSpawn();
	    if (IsServer) ChangeHP(maxHP);
    }

    public void ChangeHP(float amount)
    {
        if (isAlive)
        {
            hp.Value += amount;

            if (hp.Value >= maxHP)
	            hp.Value = maxHP;

            if (hp.Value <= 0)
            {
	            hp.Value = 0;
                isAlive = false;
                OnYouDied();
            }

            OnChangeHealth(hp.Value);
        }
    }

    [Button]
    public void ChangeHealthButton()
    {
        ChangeHP(0);
    }

    [Button]
    public void Die()
    {
        ChangeHP(-1000000000);
    }

    public void Respawn()
    {
        OnSpawn();
        isAlive = true;
        ChangeHP(maxHP);
    }

    public event Action Spawn; 

    private void OnSpawn()
    {
        Spawn?.Invoke();
    }

    public event Action<float> ChangeHealth; 

    private void OnChangeHealth(float x)
    {
        ChangeHealth?.Invoke(x);
    }
    
    public event Action<GameObject> YouDied;

    private void OnYouDied()
    {
        YouDied?.Invoke(gameObject);
    }

}