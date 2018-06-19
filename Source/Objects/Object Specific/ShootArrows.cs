using FlaxEngine;
using System.Linq;

namespace FlaxTimeNexus
{
	/// <summary>
	/// Makes a dart trap wall shoot every arrow when someone enters the trigger area
	/// </summary>
	public class ShootArrows : Script
	{
		public Actor DartTrapWall;
		public float Damage;

		public Vector3 LinearVelocity = Vector3.Zero;

		private void OnTriggerEnter(Collider collider)
		{
			if (DartTrapWall && collider.GetScript<Player>() != null)
			{
				foreach (Actor trap in DartTrapWall.GetChildren().Where(a => a.Name.Contains("Trap")))
				{
					RigidBody arrow = trap.GetChild<RigidBody>();
					foreach (Collider arrowCollider in arrow.GetChildren<Collider>())
					{
						var damager = arrowCollider.AddScript<Damager>();
						damager.HitDamage = Damage;

						arrowCollider.IsTrigger = true;

						var onHitDisableSimulation = arrowCollider.AddScript<OnHitDisableSimulation>();
						onHitDisableSimulation.arrow = arrow;
					}

					arrow.LinearVelocity = Vector3.Transform(LinearVelocity, arrow.Orientation);
				}
			}
		}

		private class OnHitDisableSimulation : Script
		{

			private bool _firstHit = false;
			public RigidBody arrow;

			private void OnTriggerEnter(Collider collider)
			{
				Player player = collider?.GetScript<Player>();
				if (player)
				{
					arrow.EnableGravity = true;
				}
				else if (this.Actor.LocalPosition.Length < 100f) //Cheap hack
				{
					arrow.EnableSimulation = false;
				}

			}

			private void OnCollisionEnter(Collision collision)
			{
				OnTriggerEnter(collision.OtherCollider);
			}
		}
	}
}
