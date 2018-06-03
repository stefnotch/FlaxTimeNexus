using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus.Source.Editor
{
	[CustomEditor(typeof(SDateTime))]
	public class SDateTimeEditor : GenericEditor
	{
		/*public override DisplayStyle Style
		{
			get
			{
				return DisplayStyle.InlineIntoParent;
			}
		}*/


		IntegerValueElement year;
		IntegerValueElement day;
		IntegerValueElement hour;
		IntegerValueElement minute;
		IntegerValueElement second;
		public override void Initialize(LayoutElementsContainer layout)
		{

			base.Initialize(layout);
			((FlaxEngine.GUI.DropPanel)(layout.Control)).Open(false);

			//TODO: Change the null handling?
			if (Values[0] == null) return;

			year = layout.IntegerValue("Year");

			day = layout.IntegerValue("Day");
			day.SetLimits(new LimitAttribute(0, SDateTime.YearToDays - 1));

			hour = layout.IntegerValue("Hour");
			hour.SetLimits(new LimitAttribute(0, SDateTime.DayToHours - 1));

			minute = layout.IntegerValue("Minute");
			minute.SetLimits(new LimitAttribute(0, SDateTime.HourToMinutes - 1));

			second = layout.IntegerValue("Second");
			second.SetLimits(new LimitAttribute(0, SDateTime.MinuteToSeconds - 1));

			if (Values.IsSingleObject && Values.Type == typeof(SDateTime))
			{
				((SDateTime)Values[0]).Decompose(out int years, out int days, out int hours, out int minutes, out int seconds);
				year.Value = years;
				day.Value = days;
				hour.Value = hours;
				minute.Value = minutes;
				second.Value = seconds;
			}
			year.IntValue.EditEnd += EditEnd;
			day.IntValue.EditEnd += EditEnd;
			hour.IntValue.EditEnd += EditEnd;
			minute.IntValue.EditEnd += EditEnd;
			second.IntValue.EditEnd += EditEnd;
		}

		private void EditEnd()
		{

			this.SetValue(new SDateTime(year.Value, day.Value, hour.Value, minute.Value, second.Value));
		}

		~SDateTimeEditor()
		{
			if (year != null)
			{
				year.IntValue.EditEnd -= EditEnd;
			}
			if (day != null)
			{
				day.IntValue.EditEnd -= EditEnd;
			}
			if (hour != null)
			{
				hour.IntValue.EditEnd -= EditEnd;
			}
			if (minute != null)
			{
				minute.IntValue.EditEnd -= EditEnd;
			}
			if (second != null)
			{
				second.IntValue.EditEnd -= EditEnd;
			}
		}
	}
}
