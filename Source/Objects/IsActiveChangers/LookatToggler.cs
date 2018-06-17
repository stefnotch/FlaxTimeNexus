using FlaxEngine;

namespace FlaxTimeNexus
{
	/// <summary>
	/// Activates an actor while this actor is being looked at
	/// </summary>
	public class LookatToggler : Script, ILookatTrigger, IIsActiveToggler
	{
		public Actor ToActivate { get; set; }
		public Actor ToDeactivate { get; set; }

		public virtual void OnLookatEnter(RayCastHit hitResult)
		{
			if (ToActivate)
			{
				ToActivate.IsActive = true;
			}
		}

		public virtual void OnLookatExit(RayCastHit hitResult)
		{
			if (ToDeactivate)
			{
				ToDeactivate.IsActive = false;
			}
		}

		public virtual void OnLookatStay(RayCastHit hitResult)
		{
			// /ignore
		}
	}
}
