using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxTimeNexus;

namespace FlaxTimeNexus
{
	public class Damager : Script
	{
		public float Damage { get; set; }

		private void OnCollisionEnter(Collision collision)
		{
			Player player = collision.OtherCollider.GetScript<Player>();
			if (player)
			{
				player.Health -= Damage;
			}
		}
	}
}
