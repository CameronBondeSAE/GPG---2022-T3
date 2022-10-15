using System;
using UnityEngine;

namespace Team_Members.Cam
{
	public class CamVision : MonoBehaviour
	{
		public int rays = 10;
		public float spacingScale = 1f;

		private void FixedUpdate()
		{
			for (int i = -rays; i < rays; i++)
			{
				// Very simple. Doesn't take any tilting or pitching into account, but is fine for horizontal only AIs
				Vector3 dir = Quaternion.Euler(0, i*spacingScale, 0) * transform.forward;
				
				Debug.DrawRay(transform.position, dir * 10f, Color.green);
			}
		}
	}
}