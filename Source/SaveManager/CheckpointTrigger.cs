using FlaxEngine;

namespace FlaxTimeNexus
{
	public class CheckpointTrigger : Script
	{
		void OnTriggerEnter(Collider other)
		{
			Player player = other.GetScript<Player>();
			if (player)
			{
				//TODO: Print a message ("saving...") or a little icon
				player.Save();
			}
		}
	}
}
