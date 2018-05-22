using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTemplate
{
	public class PlayerLookat : Script
	{
		public Camera Camera;
		public float MaxDistance = 100f * 30f;

		RayCastHit _lastHit;
		void Update()
		{
			//TODO: How to deal with children???? (capturing, bubbling,...)
			RayCastHit[] hits = Physics.RayCastAll(Camera.Position, Camera.Direction, MaxDistance, this.Actor.Layer);

			Array.Sort(hits, (a, b) => (int)((a.Distance - b.Distance) * 10));

			if (hits.Length > 0)
			{
				RayCastHit hit = hits[0];
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
				ILookatTrigger lookatTrigger = s as ILookatTrigger;
				if (lookatTrigger != null)
				{
					yield return lookatTrigger;
				}
			}
		}
	}
}