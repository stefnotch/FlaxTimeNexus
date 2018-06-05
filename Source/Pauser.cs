using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FlaxTimeNexus
{
	public class Pauser : Script
	{
		public Actor Player;
		public Actor PauseRoom;

		[NoSerialize]
		public bool Paused
		{
			get
			{
				return _isPaused;
			}
			set
			{
				_isPaused = value;
				if (_isPaused)
				{
					Time.TimeScale = 0;
					if(PauseRoom && Player)
					{
						PauseRoom.Position = Player.Position;
						PauseRoom.Orientation = Player.Orientation;
						PauseRoom.IsActive = true;
					}
					//Screen.CursorLock = CursorLockMode.None;
					//Screen.CursorVisible = true;
				}
				else
				{
					Time.TimeScale = 1;
					if (PauseRoom && Player)
					{
						PauseRoom.IsActive = false;
					}
					//Screen.CursorLock = CursorLockMode.Locked;
					//Screen.CursorVisible = false;
				}
			}

		}

		[NoSerialize]
		InputEvent Pause = new InputEvent("Pause");

		[NoSerialize]
		bool _isPaused;
		private void Start()
		{
			if (Pause != null)
			{
				Pause.Triggered += Pause_Triggered;
			}

			//Paused = false;
		}

		private void Pause_Triggered()
		{
			Paused = !Paused;
		}

		private void OnDisable()
		{
			if (Pause != null)
			{
				Pause.Triggered -= Pause_Triggered;
			}
		}

	}
}
