using FlaxEngine;

namespace FlaxTimeNexus
{
	public class ForceField : Script
	{
		public Vector3 LocalDirection = Vector3.Forward;
		public float Strength = 1;

		private void Start()
		{
			if (!(this.Actor is Collider)) Debug.LogError(nameof(ForceField) + ".cs can only be attached to Trigger-Colliders!");
			// Here you can add code that needs to be called when script is created
		}

		private void OnTriggerStay(Collider other)
		{
			Vector3 velocityIncrement = Vector3.Transform(LocalDirection, Actor.Orientation) * Strength / Time.DrawFPS;
			if (other is CharacterController player)
			{
				player.Move(player.Velocity + velocityIncrement * 10f);
			}
			else
			{
				other.AttachedRigidBody.LinearVelocity += velocityIncrement;
			}
		}
	}
}
