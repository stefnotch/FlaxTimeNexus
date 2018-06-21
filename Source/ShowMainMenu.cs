using FlaxEngine;

namespace FlaxTimeNexus.Source
{
	public class ShowMainMenu : Script
	{
		public Actor MainMenu;

		private void Start()
		{

			MainMenu.IsActive = true;
			Time.TimeScale = 0;
		}
	}
}
