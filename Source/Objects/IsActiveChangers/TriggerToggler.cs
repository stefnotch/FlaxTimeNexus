﻿using FlaxEngine;

namespace FlaxTimeNexus
{
	public class TriggerToggler : Script, IIsActiveToggler
	{
		[Tooltip("Only the player can activate this trigger")]
		public bool PlayerTrigger = true;

		[Tooltip("Gets activated when an actor enters the trigger")]
		public Actor ToActivate { get; set; }

		[Tooltip("Gets deactivated when an actor leaves the trigger")]
		public Actor ToDeactivate { get; set; }

		private void OnTriggerEnter(Collider c)
		{
			if (PlayerTrigger && !c.GetScript<Player>()) return;

			if (ToActivate) ToActivate.IsActive = true;
		}

		private void OnTriggerExit(Collider c)
		{
			if (PlayerTrigger && !c.GetScript<Player>()) return;

			if (ToDeactivate) ToDeactivate.IsActive = false;
		}
	}
}
