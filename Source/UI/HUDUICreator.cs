using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxTimeNexus.Source.UI
{
	public class HUDUICreator : UICreator
	{
		public override ContainerControl CreateUI(Vector2 size)
		{
			ContainerControl uiRoot = new ContainerControl
			{
				BackgroundColor = Color.Zero,
				Size = size
			};

			ProgressBar progressBar = new ProgressBar(0, 0, 100);
			uiRoot.AddChild(progressBar);
			//progressBar.
			return uiRoot;
		}
	}
}
