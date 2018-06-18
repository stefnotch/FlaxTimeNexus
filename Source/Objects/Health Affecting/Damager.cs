using FlaxEngine;

namespace FlaxTimeNexus
{
	/// <summary>
	/// Damages a player
	/// </summary>
	public class Damager : Script
	{
		/// <summary>
		/// Amount of damage on hit, independent of the DamagePerSecond
		/// </summary>
		public float HitDamage { get; set; }

		/// <summary>
		/// Amount of damage per second, independent of the HitDamage
		/// </summary>
		public float DamagePerSecond { get; set; }

		private void OnCollisionEnter(Collision collision)
		{
			OnTriggerEnter(collision.OtherCollider);
		}

		private void OnTriggerEnter(Collider collider)
		{
			Player player = collider?.GetScript<Player>();
			if (player)
			{
				player.Health -= HitDamage;
			}
		}

		private void OnCollisionStay(Collision collision)
		{
			OnTriggerStay(collision.OtherCollider);
		}

		private void OnTriggerStay(Collider collider)
		{
			Player player = collider?.GetScript<Player>();
			if (player)
			{
				player.Health -= DamagePerSecond / Time.PhysicsFPS;
			}
		}
	}
}
