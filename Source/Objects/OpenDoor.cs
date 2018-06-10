using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxTimeNexus
{
	public class OpenDoor : Script, ILookatTrigger
	{
		public Actor Door;
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
			if (Interact.Active)
			{
				
			}
		}

		private void Start()
		{
			// Here you can add code that needs to be called when script is created
		}

		private void Update()
		{
			//if(_opening)
			{
				
			}
		}
	}
}
