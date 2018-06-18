using FlaxEngine;

namespace FlaxTimeNexus.Source.Objects
{
	public class OnEnableVelocity : Script
	{
		public Vector3 LinearVelocity = Vector3.Zero;

		//TODO: Auto set this/make it only possible to attach this to a rigidbody
		public RigidBody RigidBody;

		private void OnEnable()
		{
			if (RigidBody) RigidBody.LinearVelocity += Vector3.Transform(LinearVelocity, Actor.Orientation);
		}
	}
}
