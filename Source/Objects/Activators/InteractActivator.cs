﻿using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	/// <summary>
	/// Activates an actor when the interact key gets pressed
	/// Does NOT deactivate it afterwards
	/// </summary>
	public class InteractActivator : Script, ILookatTrigger, IActivator
	{
		public Actor ToActivate { get; set; }
		/// <summary>
		/// How close the player needs to be to interact with an object
		/// Note: Limited by the PlayerLookat MaxDistance 
		/// </summary>
		public float InteractionRadius = 100f;

		InputEvent Interact = new InputEvent("Interact");

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
				ToActivate.IsActive = true;
			}
		}
	}
}