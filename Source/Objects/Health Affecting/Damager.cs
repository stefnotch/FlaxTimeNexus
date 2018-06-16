using FlaxEngine;

namespace FlaxTimeNexus
{
	public class Damager : Script
	{
		public float Damage { get; set; }

		private void OnCollisionEnter(Collision collision)
		{
			Debug.Log("Ok");
			Player player = collision.OtherCollider.GetScript<Player>();
			if (player)
			{
				player.Health -= Damage;
			}
		}

		private void OnTriggerEnter(Collider collider)
		{
			Debug.Log("OkT");
			Player player = collider.GetScript<Player>();
			if (player)
			{
				player.Health -= Damage;
			}
		}
	}
}
