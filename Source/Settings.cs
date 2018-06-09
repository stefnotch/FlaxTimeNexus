using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	/// <summary>
	/// Cascading settings
	/// </summary>
	/// 
	//[Obsolete("The default time isn't used anywhere, it depends on which actor (TimeContainer --> children) is activated.")]
	public class Settings : Script
	{
		[Serialize]
		private SDateTime _defaultTime = SDateTime.Zero;

		[NoSerialize]
		public SDateTime DefaultTime
		{
			get
			{
				return _defaultTime;
			}
			set
			{
				_defaultTime = value;
				/*if (this.Actor)
				{
					foreach (Actor actor in this.Actor.DepthFirst((actor) => actor.GetScript<Settings>() == null))
					{

						var timeContainer = actor.GetScript<TimeContainer>();
						if (timeContainer)
						{
							timeContainer.Time = value;
						}

					}
				}*/
			}
		}
	}
}
