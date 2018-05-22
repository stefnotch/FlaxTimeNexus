using System;
using System.Collections.Generic;
using FlaxEngine;

namespace BasicTemplate
{
	public class Pauser : Script
	{
		InputEvent Pause = new InputEvent("Pause");
		bool _isPaused;
		private void Start()
		{
			if (Pause != null)
			{
				Pause.Triggered += Pause_Triggered;
			}
		}

		private void Pause_Triggered()
		{
			_isPaused = !_isPaused;
			if (_isPaused)
			{
				Time.TimeScale = 0;

				Screen.CursorLock = CursorLockMode.None;
				Screen.CursorVisible = true;
			}
			else
			{
				Time.TimeScale = 1;
			}
		}

	}
}
