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

		private float _health;
		public float Health { get => _health;
			set
			{
				_health = value;
				if (_health <= 0) Respawn();
			}
		}

		private readonly List<Checkpoint> _checkpoints = new List<Checkpoint>();

		private void Start()
		{
			Health = DefaultHealth;
		}

		private void Update()
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
