using Marcus;
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
		};

		public HQType type;

		private GameObject myself;

		private Renderer rend;

		public float _HQRadius;

		//tracks items deposited
		private int itemCount;

		//when this many items have been deposited, fire victory event
		public int itemVictory;

		//tracks Team
		//Human or Alien
		//Neutral could be destroyed bases?
		private int HQInt;
		private string HQString;

		private void Start()
		{

			myself = this.GameObject();

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
			
			DestroyLand(_HQRadius);
		}

		public void DestroyLand(float x)
		{
			_HQRadius = x;

			Collider[] colliders = Physics.OverlapSphere(transform.position, _HQRadius);

			foreach (Collider obj in colliders)
			{
				if (obj.GetComponent<Health>() != null)
				{
					obj.GetComponent<Health>().ChangeHP(-1000000);
				}
			}
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
			Destroy(myself);
		}


	}
}