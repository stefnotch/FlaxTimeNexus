using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

namespace FlaxTimeNexus.Source.Editor
{
	[CustomEditor(typeof(SDateTime))]
	public class SDateTimeEditor : GenericEditor
	{

		IntegerValueElement year;
		IntegerValueElement day;
		IntegerValueElement hour;
		IntegerValueElement minute;
		IntegerValueElement second;
		public override void Initialize(LayoutElementsContainer layout)
		{

			base.Initialize(layout);
			((FlaxEngine.GUI.DropPanel)(layout.Control)).Open(false);

			year = layout.IntegerValue("Year");

			day = layout.IntegerValue("Day");
			day.SetLimits(new LimitAttribute(0, SDateTime.YearToDays - 1));

			hour = layout.IntegerValue("Hour");
			hour.SetLimits(new LimitAttribute(0, SDateTime.DayToHours - 1));

			minute = layout.IntegerValue("Minute");
			minute.SetLimits(new LimitAttribute(0, SDateTime.HourToMinutes - 1));

			second = layout.IntegerValue("Second");
			second.SetLimits(new LimitAttribute(0, SDateTime.MinuteToSeconds - 1));

			year.IntValue.EditEnd += EditEnd;
			day.IntValue.EditEnd += EditEnd;
			hour.IntValue.EditEnd += EditEnd;
			minute.IntValue.EditEnd += EditEnd;
			second.IntValue.EditEnd += EditEnd;

			//FlaxEditor.Editor.Instance.Windows.ContentWin.CurrentViewFolder.NamePath
		}

		private void EditEnd()
		{
			//Takes the user input
			SetValue(new SDateTime(year.Value, day.Value, hour.Value, minute.Value, second.Value));
		}

		public override void Refresh()
		{
			base.Refresh();
			//Displays the value
			if (!HasDifferentValues)
			{
				if (Values[0] is SDateTime dateTime)
				{
					dateTime.Decompose(out int years, out int days, out int hours, out int minutes, out int seconds);
					year.Value = years;
					day.Value = days;
					hour.Value = hours;
					minute.Value = minutes;
					second.Value = seconds;
				}
			}
			else
			{

				//TODO:
				//HasDifferentValues
			}

		}
	}
}
