using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTemplate
{
	/// <summary>
	/// Cascading settings
	/// </summary>
	public class Settings : Script
	{
		[Serialize]
		private DateTime _defaultTime;

		[NoSerialize]
		public DateTime DefaultTime
		{
			get
			{
				return _defaultTime;
			}
			set
			{
				_defaultTime = value;
				/*if (Actor)
				{
					Actor.DepthFirst((actor) =>
					{
						var timeContainer = actor.GetScript<TimeContainer>();
						if (timeContainer)
						{
							timeContainer.Time = value;
						}
					},
					(actor) => actor.GetScript<Settings>() == null);
				}*/
			}
		}



	}
}
