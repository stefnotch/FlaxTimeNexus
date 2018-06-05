using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxTimeNexus
{
	public class MainMenuUICreator : UICreator
	{
		public Pauser Pauser;
		public Actor MainMenu;

		public override ContainerControl CreateUI(Vector2 size)
		{
			ContainerControl uiRoot = new ContainerControl
			{
				BackgroundColor = Color.White,
				Size = size
			};

			UniformGridPanel buttonContainer = new UniformGridPanel
			{
				SlotsHorizontally = 1,
				SlotPadding = new Margin(0, 0, size.AvgValue * 0.05f, size.AvgValue * 0.05f)
			};
			//Resume
			Button startButton = new Button
			{
				Text = "Start",
				BackgroundColor = Color.Gray
			};
			startButton.Clicked += () =>
			{
				if (Pauser)
				{
					Pauser.Paused = false;
					MainMenu.IsActive = false;
				}
			};
			buttonContainer.AddChild(startButton);

			//Exit
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

			buttonContainer.SlotsVertically = buttonContainer.ChildrenCount;

			//For centering the column of buttons
			GridPanel centeringGrid = new GridPanel()
			{
				DockStyle = DockStyle.Fill,
				ColumnFill = new float[] { 0.15f, 0.7f, 0.15f }
			};
			centeringGrid.RowFill[0] = 1f;

			centeringGrid.AddChild(new Spacer(0, 0));
			centeringGrid.AddChild(buttonContainer);
			centeringGrid.AddChild(new Spacer(0, 0));

			uiRoot.AddChild(centeringGrid);

			uiRoot.PerformLayout(true);
			return uiRoot;
		}
	}
}
