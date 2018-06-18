using FlaxEngine;

namespace FlaxTimeNexus
{
	public class TriggerLeaveToggler : Script, IIsActiveToggler
	{
		[Tooltip("Only the player can activate this trigger")]
		public bool PlayerTrigger = true;

		[Tooltip("Gets activated when an actor leaves the trigger")]
		public Actor ToActivate { get; set; }

		[Tooltip("Gets deactivated when an actor leaves the trigger")]
		public Actor ToDeactivate { get; set; }

		private void OnTriggerExit(Collider c)
		{
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
