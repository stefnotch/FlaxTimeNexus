using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTemplate
{
	/// <summary>
	/// Activates an actor while this actor is being looked at
	/// </summary>
	public class LookatActivator : Script, ILookatTrigger
	{
		public Actor ToActivate;

		public void OnLookatEnter(RayCastHit hitResult)
		{
			if(ToActivate)
			{
				ToActivate.IsActive = true;
			}
		}

		public void OnLookatExit(RayCastHit hitResult)
		{
			if (ToActivate)
			{
				ToActivate.IsActive = false;
			}
		}

		public void OnLookatStay(RayCastHit hitResult)
		{
			// /ignore
		}
	}
}
