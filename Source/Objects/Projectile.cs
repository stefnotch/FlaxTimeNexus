using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	public class Projectile : Script
	{
		public float Damage = 0f;
		void OnCollisionEnter(Collision collision)
		{
			Player player = collision.OtherCollider.GetScript<Player>();
			if (player != null)
			{
				player.Health -= Damage;
				if (player.Health <= 0) player.Respawn();
			}
		}
	}
}
