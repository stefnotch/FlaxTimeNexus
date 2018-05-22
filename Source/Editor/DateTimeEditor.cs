using FlaxEditor.CustomEditors;
using FlaxEditor.CustomEditors.Editors;
using FlaxEditor.CustomEditors.Elements;
using FlaxEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTemplate.Source.Editor
{
	[CustomEditor(typeof(DateTime))]
	public class DateTimeEditor : GenericEditor
	{
		/*public override DisplayStyle Style
		{
			get
			{
				return DisplayStyle.InlineIntoParent;
			}
		}*/


		IntegerValueElement year;
		public override void Initialize(LayoutElementsContainer layout)
		{

			base.Initialize(layout);
			((FlaxEngine.GUI.DropPanel)(layout.Control)).Open(false);

			year = layout.IntegerValue("Year");
			year.SetLimits(new LimitAttribute(1));
			if (Values.IsSingleObject && Values.Type == typeof(DateTime))
			{
				year.Value = ((DateTime)Values[0]).Year;
			}
			year.IntValue.EditEnd += IntValue_EditEnd;
		}

		private void IntValue_EditEnd()
		{

			this.SetValue(new DateTime(year.IntValue.Value, 1, 1));


		}

		~DateTimeEditor()
		{
			if (year != null)
			{
				year.IntValue.EditEnd -= IntValue_EditEnd;
			}
		}
	}
}
