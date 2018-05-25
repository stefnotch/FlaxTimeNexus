﻿using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxTimeNexus
{
	public class TriggerActivator : Script, IActivator
	{
		public Actor ToActivate { get; set; }

		void OnTriggerEnter(Collider c)
		{
			ToActivate.IsActive = true;
		}

		void OnTriggerExit(Collider c)
		{
			ToActivate.IsActive = false;
		}
	}
}