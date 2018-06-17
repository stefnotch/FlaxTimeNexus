using FlaxEngine;

namespace FlaxTimeNexus
{
	public class TriggerToggler : Script, IIsActiveToggler
	{
		public Actor ToActivate { get; set; }
		public Actor ToDeactivate { get; set; }

		private void OnTriggerEnter(Collider c)
		{
			if (ToActivate) ToActivate.IsActive = true;
		}

		private void OnTriggerExit(Collider c)
		{
			if (ToDeactivate) ToDeactivate.IsActive = false;
		}
	}
}
