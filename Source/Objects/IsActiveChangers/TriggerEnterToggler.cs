using FlaxEngine;

namespace FlaxTimeNexus
{
	public class TriggerEnterToggler : Script, IIsActiveToggler
	{
		[Tooltip("Only the player can activate this trigger")]
		public bool PlayerTrigger = true;

		[Tooltip("Gets activated when an actor enters the trigger")]
		public Actor ToActivate { get; set; }

		[Tooltip("Gets deactivated when an actor enters the trigger")]
		public Actor ToDeactivate { get; set; }

		private void OnTriggerEnter(Collider c)
		{
			//If only a player can trigger it, but it's not a player, don't trigger it!
			if (PlayerTrigger && !c.GetScript<Player>()) return;

			if (ToActivate == ToDeactivate)
			{
				if (ToActivate)
				{
					ToActivate.IsActive = !ToActivate.IsActive;
				}
			}
			else
			{
				if (ToActivate) ToActivate.IsActive = true;
				if (ToDeactivate) ToDeactivate.IsActive = false;
			}
		}
	}
}
