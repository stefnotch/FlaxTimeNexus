using FlaxEngine;

namespace FlaxTimeNexus
{
	public class LookatDistanceToggler : LookatToggler
	{
		/// <summary>
		/// How close does the player have to be to activate an actor
		/// </summary>
		public float MinDistance = 100f;

		private bool _toActivateState;
		private bool _toDeactivateState;

		private void Start()
		{
			_toActivateState = ToActivate.IsActive;
			_toDeactivateState = ToDeactivate.IsActive;
		}

		public override void OnLookatEnter(RayCastHit hitResult)
		{
			//ignore
		}

		public override void OnLookatExit(RayCastHit hitResult)
		{
			DisableActors();
		}

		public override void OnLookatStay(RayCastHit hitResult)
		{
			if (hitResult.Distance <= MinDistance)
			{
				EnableActors();
			}
			else
			{
				DisableActors();
			}
		}

		private void EnableActors()
		{
			if (ToActivate && _toActivateState == false)
			{
				ToActivate.IsActive = true;
				_toActivateState = true;
				if (ToActivate == ToDeactivate)
				{
					_toDeactivateState = true;
				}
			}
		}

		private void DisableActors()
		{
			if (ToDeactivate && _toDeactivateState == true)
			{
				ToDeactivate.IsActive = false;
				_toDeactivateState = false;
				if (ToActivate == ToDeactivate)
				{
					_toActivateState = false;
				}
			}
		}

	}
}
