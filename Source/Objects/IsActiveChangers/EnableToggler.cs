using FlaxEngine;

namespace FlaxTimeNexus
{
	public class EnableToggler : Script, IIsActiveToggler
	{
		public Actor ToActivate { get; set; }
		public Actor ToDeactivate { get; set; }

		//TODO: Ask about this vvv
		//[ExecuteInEditMode]
		private void OnEnable()
		{
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
