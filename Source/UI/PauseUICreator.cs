using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxTimeNexus.Source.UI
{
	public class PauseUICreator : UICreator
	{
		public Pauser Pauser;
		public override ContainerControl CreateUI(Vector2 size)
		{

			ContainerControl uiRoot = new ContainerControl
			{
				BackgroundColor = Color.White,
				Size = size
			};

			VerticalPanel buttonContainer = new VerticalPanel
			{
				Size = size
			};
			buttonContainer.SetLocation(0, 0);
			buttonContainer.BackgroundColor = Color.Green;

			Button resumeButton = new Button();
			resumeButton.Text = "Resume";
			resumeButton.BackgroundColor = Color.Gray;
			resumeButton.Clicked += () =>
			{

				if (Pauser)
				{
					Pauser.Paused = false;
				}
			};
			buttonContainer.AddChild(resumeButton);

			Button exitButton = new Button
			{
				Text = "Exit",
				BackgroundColor = Color.Gray
			};
			exitButton.Clicked += () =>
			{
				Application.Exit();
			};
			buttonContainer.AddChild(exitButton);

			//buttonContainer.PerformLayout(true);

			uiRoot.AddChild(buttonContainer);

			uiRoot.PerformLayout(true);
			return uiRoot;
		}
	}
}
