using FlaxEngine;

namespace FlaxTimeNexus
{
	public class InteractToggler : Script, ILookatTrigger, IIsActiveToggler
	{
		public Actor ToActivate { get; set; }
		public Actor ToDeactivate { get; set; }

		/// <summary>
		/// How close the player needs to be to interact with an object
		/// Note: Limited by the PlayerLookat MaxDistance 
		/// </summary>
		public float InteractionRadius = 100f;
		private InputEvent Interact = new InputEvent("Interact");

		public void OnLookatEnter(RayCastHit hitResult)
		{

		}

		public void OnLookatExit(RayCastHit hitResult)
		{

		}

		public void OnLookatStay(RayCastHit hitResult)
		{
			if (Interact.Active && hitResult.Distance <= InteractionRadius)
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
}
