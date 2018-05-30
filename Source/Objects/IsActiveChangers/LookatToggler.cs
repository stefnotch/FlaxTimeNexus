using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	/// <summary>
	/// Activates an actor while this actor is being looked at
	/// </summary>
	public class LookatToggler : Script, ILookatTrigger, IIsActiveChanger
	{
		public Actor ToActivate { get; set; }

		public virtual void OnLookatEnter(RayCastHit hitResult)
		{
			if(ToActivate)
			{
				ToActivate.IsActive = true;
			}
		}

		public virtual void OnLookatExit(RayCastHit hitResult)
		{
			if (ToActivate)
			{
				ToActivate.IsActive = false;
			}
		}

		public virtual void OnLookatStay(RayCastHit hitResult)
		{
			// /ignore
		}
	}
}
