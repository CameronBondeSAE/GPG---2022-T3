using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Door : NetworkBehaviour
{
	public NetworkVariable<bool> isOpen;

	// Client side view stuff. For late joining
	public void Awake()
	{
		isOpen.OnValueChanged += OnValueChanged;
	}

	private void OnValueChanged(bool previousvalue, bool newvalue)
	{
		if (newvalue)
			OpenClientRpc();
		else
			CloseClientRpc();
	}

	// Normally server calls these
	[ClientRpc(Delivery = RpcDelivery.Reliable)]
	public void OpenClientRpc()
	{
		isOpen.Value = true;
		transform.rotation = Quaternion.Euler(0, 90, 0);
	}

	[ClientRpc]
	public void CloseClientRpc()
	{
		isOpen.Value = false;
		transform.rotation = Quaternion.Euler(0, 0, 0);
	}
}