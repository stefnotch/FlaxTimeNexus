using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine;

namespace FlaxTimeNexus
{
	public class LookatGUIToggler : LookatToggler
	{
		/// <summary>
		/// How close does the player have to be to activate an actor
		/// </summary>
		public float MinDistance = 100f;

		bool _isActivated;

		void Start()
		{
			if (ToActivate)
			{
				_isActivated = ToActivate.IsActive;
			}
		}

		public override void OnLookatEnter(RayCastHit hitResult)
		{
			//ignore
		}

		public override void OnLookatExit(RayCastHit hitResult)
		{
			if (_isActivated && ToActivate)
			{
				ToActivate.IsActive = false;
				_isActivated = false;
			}
		}

		public override void OnLookatStay(RayCastHit hitResult)
		{
			if (hitResult.Distance <= MinDistance)
			{
				if(!_isActivated)
				{
					ToActivate.IsActive = true;
					_isActivated = true;
				}
			}
			else
			{
				if (_isActivated)
				{
					ToActivate.IsActive = false;
					_isActivated = false;
				}
			}

		}

	}
}
