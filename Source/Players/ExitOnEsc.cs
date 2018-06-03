using FlaxEngine;

namespace FlaxTimeNexus
{
	public class ExitOnEsc : Script
	{
		private void Update()
		{
			if (Input.GetKeyUp(Keys.Escape))
			{
				Application.Exit();
			}
		}
	}
}
