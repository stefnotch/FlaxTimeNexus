using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxTimeNexus.Source.UI
{
	public class PauseUICreator : UICreator
	{
		public Pauser Pauser;

		public Player Player;

		public override ContainerControl CreateUI(Vector2 size)
		{
			Button fake = new Button();

			var largerFont = fake.Font.Asset.CreateFont(30);

			ContainerControl uiRoot = new ContainerControl
			{
				BackgroundColor = Color.White,
				Size = size
			};

			UniformGridPanel buttonContainer = new UniformGridPanel
			{
				SlotsHorizontally = 1,
				SlotPadding = new Margin(0, 0, size.AvgValue * 0.01f, size.AvgValue * 0.01f)
			};
			//Resume
			Button resumeButton = new Button
			{
				Text = "Resume",
				BackgroundColor = Color.Gray,
				Font = largerFont
			};
			resumeButton.Clicked += () =>
			{
				if (Pauser)
				{
					Pauser.Paused = false;
				}
			};
			buttonContainer.AddChild(resumeButton);

			//Save
			Button saveButton = new Button
			{
				Text = "Save",
				BackgroundColor = Color.Gray,
				Font = largerFont
			};
			saveButton.Clicked += () => Player?.Save();
			buttonContainer.AddChild(saveButton);

			//Load
			Button loadButton = new Button
			{
				Text = "Load",
				BackgroundColor = Color.Gray,
				Font = largerFont
			};
			loadButton.Clicked += () =>
			{
				Player?.Respawn();
				Pauser.Paused = false;
			};
			buttonContainer.AddChild(loadButton);

			//Settings
			Button settingsButton = new Button
			{
				Text = "Settings",
				BackgroundColor = Color.Gray,
				Font = largerFont
			};
			settingsButton.Clicked += () =>
			{
				Debug.Log("Not implemented");
			};
			buttonContainer.AddChild(settingsButton);

			//Exit
			Button exitButton = new Button
			{
				Text = "Exit",
				BackgroundColor = Color.Gray,
				Font = largerFont
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
