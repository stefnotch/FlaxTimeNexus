using FlaxEngine;
using System.Collections.Generic;

namespace FlaxTimeNexus
{
	public class PlayerLookat : Script
	{
		public Camera Camera;
		public float MaxDistance = 100f * 30f;
		private RayCastHit _lastHit;

		private void Update()
		{
			//TODO: How to deal with children/parents???? (capturing, bubbling,...)
			if (Physics.RayCast(Camera.MainCamera.Position, Camera.MainCamera.Direction, out RayCastHit hit, MaxDistance, this.Actor.Layer, hitTriggers: false))
			{
				bool newCollider = _lastHit.Collider != hit.Collider;

				if (newCollider)
				{
					foreach (ILookatTrigger lookatTrigger in GetLookatTriggers(_lastHit.Collider))
					{
						lookatTrigger.OnLookatExit(_lastHit);
					}
				}

				foreach (ILookatTrigger lookatTrigger in GetLookatTriggers(hit.Collider))
				{
					if (newCollider)
					{
						lookatTrigger.OnLookatEnter(hit);
					}
					lookatTrigger.OnLookatStay(hit);
				}
				_lastHit = hit;
			}
			else
			{
				foreach (ILookatTrigger lookatTrigger in GetLookatTriggers(_lastHit.Collider))
				{
					lookatTrigger.OnLookatExit(_lastHit);
				}
				_lastHit = default(RayCastHit);
			}
		}

		private IEnumerable<ILookatTrigger> GetLookatTriggers(Collider c)
		{
			if (c == null) yield break;

			foreach (Script s in c.GetScripts<Script>())
			{
				if (s is ILookatTrigger lookatTrigger)
				{
					yield return lookatTrigger;
				}
			}
		}
	}
}