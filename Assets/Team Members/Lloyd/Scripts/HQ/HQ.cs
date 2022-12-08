using System;
using Marcus;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Lloyd
{

	public class HQ : MonoBehaviour
	{
		private bool isActive;

		public enum HQType
		{
			Neutral,
			Humans,
			Aliens
		}

		public HQType type;

		private Renderer rend;

		//tracks items deposited
		private int itemCount;

		//when this many items have been deposited, fire victory event
		public int itemVictory;

		//tracks Team
		//Human or Alien
		//Neutral could be destroyed bases?
		private int HQInt;
		private string HQString;

		private void Awake()
		{
			Collider[] obstructions = Physics.OverlapSphere(transform.position, 10);
			foreach (Collider item in obstructions)
			{
				if (item.GetComponent<Health>() != null)
				{
					item.GetComponent<Health>().ChangeHP(-10000000);
				}
			}
		}

		private void Start()
		{
			HQInt = (int) type;
			HQString = type.ToString();
			itemCount = 0;

			// rend = this.GetComponent<Renderer>();

			// if (HQInt == 1)
			// 	rend.material.color = Color.green;
			//
			// else if (HQInt == 2)
			// 	rend.material.color = Color.magenta;

			isActive = true;
		}

		public void ItemDeposited()
		{
			if (isActive)
			{
				//should only go up if the person depositing is aligned (humans can't deposit at alien base & vice versa)
				itemCount++;

				Debug.Log("Item deposited! " + HQString + " has " + itemCount + " items!");

				if (itemCount >= itemVictory)
					GameOver();
			}
		}

		private void GameOver()
		{
			isActive = false;
			switch (HQInt)
			{
				case 1:
					//humans win
					break;

				case 2:
					//aliebs win
					break;

			}

			Debug.Log("A stunning victory for the " + HQString + "!");
			//Victory Event
		}

		public void SetTeam(int key)
		{
			switch (key)
			{
				case 1:
					type = HQType.Humans;
					break;

				case 2:
					type = HQType.Aliens;
					break;
			}
		}

		public void KillSelf()
		{
			Destroy(gameObject);
		}


	}
}