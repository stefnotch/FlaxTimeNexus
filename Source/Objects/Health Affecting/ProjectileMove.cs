using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxTimeNexus
{
	public class ProjectileMove : Script
	{
		public float speed;
		public RigidBody rb; //This is fine

		private Vector3 _startPosition;
		private void Start()
		{
			_startPosition = rb.LocalPosition;
			rb.LinearVelocity = new Vector3(this.speed, 0, 0);
		}


		void OnCollisionEnter(Collision collision)
		{
			//Reset
			rb.LocalPosition = _startPosition;
		}
	}
}
