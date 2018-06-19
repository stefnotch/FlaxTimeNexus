using FlaxEngine;
using FlaxEngine.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace FlaxTimeNexus
{
	public class Player : Script
	{
		public float MaxHealth = 100;
		public float DefaultHealth = 100;


		//Eww
		public Material HealthBar;
		private MaterialParameter _h;
		//end of Eww

		private float _health;
		public float Health
		{
			get => _health;
			set
			{
				//Eww - Very cheap health bar
				if (_h != null)
				{
					_h.Value = value / MaxHealth;
				}
				//end of Eww

				_health = value;
				if (_health <= 0) Respawn();
			}
		}

		private readonly List<Checkpoint> _checkpoints = new List<Checkpoint>();

		private void Start()
		{
			if (HealthBar)
			{
				_h = HealthBar.GetParam("Health");
			}
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

				var fpsCamera = this.Actor.GetScript<FPSCamera>();
				if (fpsCamera) fpsCamera.SetOrientation(_checkpoints.Last().PlayerTransform.Orientation);
			}
		}

		private void OnDestroy()
		{

		}
	}
}
