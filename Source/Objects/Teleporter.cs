using FlaxEngine;

namespace FlaxTimeNexus
{
	public class Teleporter : Script
	{
		/// <summary>
		/// True: Player's position += Target.Position - Teleporter.Position;
		/// False: Player's position = Target.Position
		/// </summary>
		[Tooltip("Should the teleportation be relative?")]
		public bool IsRelative = true;

		[Tooltip("Teleportation Target")]
		public Actor Target;

		private void OnTriggerEnter(Collider collider)
		{
			Player player = collider?.GetScript<Player>();
			if (player && Target)
			{
				if (IsRelative)
				{
					player.Actor.Position += Target.Position - this.Actor.Position;
				}
				else
				{
					player.Actor.Position = Target.Position;
				}
			}
		}
	}
}
