using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxTimeNexus
{
	public class TriggerToggler : Script, IIsActiveChanger
	{
		public Actor ToActivate { get; set; }

		private void OnTriggerEnter(Collider c)
		{
			ToActivate.IsActive = true;
		}

		private void OnTriggerExit(Collider c)
		{
			ToActivate.IsActive = false;
		}
	}
}
