using FlaxEngine;

namespace FlaxTimeNexus
{
	public class DirectionalTeleporter : Script
	{
		public Quaternion ExpectedOrientation = Quaternion.Identity;

		public DirectionalTeleporter Other;

		[Tooltip("Epsilon, in degrees, both sides")]
		public float Epsilon = 90f;

		private void OnTriggerStay(Collider collider)
		{
			Player player = collider?.GetScript<Player>();
			if (player)
			{
				var angle = Quaternion.AngleBetween(player.Actor.Orientation, this.Actor.Orientation * ExpectedOrientation);
				Debug.Log("Angle1: " + angle);

				if (angle <= Epsilon)
				{
					player.Actor.Position += (Other.Actor.Position - this.Actor.Position) / 2f; //Cause it gets called twice... TODO: That's such a dirty hack!
					Debug.Log(this.Actor.Name);
					//TODO: It gets called twice!!!!
					//CharacterController
					//TODO: Orientation TP
				}
			}
		}
	}
}
