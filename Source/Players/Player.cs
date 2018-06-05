using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	public class Player : Script
	{
		public float DefaultHealth = 100;

		public float Health;

		readonly List<Checkpoint> _checkpoints = new List<Checkpoint>();

		void Start()
		{
			Health = DefaultHealth;
		}

		void Update()
		{
			
		}

		public void Save()
		{
			_checkpoints.Add(new Checkpoint() { PlayerTransform = this.Actor.Transform });
		}

		public void Respawn()
		{
			this.Health = DefaultHealth;
			if (_checkpoints.Count > 0)
			{
				this.Actor.Transform = _checkpoints.Last().PlayerTransform;
			}
		}
	}
}
