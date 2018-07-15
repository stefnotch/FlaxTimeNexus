using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxTimeNexus
{
	public class Billboard : Script
	{
		private Vector3 _up = Vector3.Up;
		private void OnEnable()
		{
			this.Actor.Orientation = Quaternion.Billboard(this.Actor.Position, Camera.MainCamera.Position, _up, Camera.MainCamera.Direction);
		}

		private void Update()
		{
			this.Actor.Orientation = Quaternion.Billboard(this.Actor.Position, Camera.MainCamera.Position, _up, Camera.MainCamera.Direction);
		}
	}
}
