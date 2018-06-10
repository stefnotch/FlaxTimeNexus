using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus.Source
{
	public class ShowMainMenu : Script
	{
		public Actor MainMenu;

		private void Start()
		{
			Time.TimeScale = 0;
			MainMenu.IsActive = true;
		}
	}
}
