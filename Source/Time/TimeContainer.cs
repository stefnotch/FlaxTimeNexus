using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	/// <summary>
	/// Used to change the time of an object
	/// Only affects direct children
	/// </summary>
	public class TimeContainer : Script
	{
		[NoSerialize]
		private SDateTime _time = SDateTime.Zero;

		[NoSerialize]
		private Actor _previousActor;

		[HideInEditor]
		[NoSerialize]
		public SDateTime Time
		{
			get
			{
				return _time;
			}
			set
			{
				_time = value;

				TimeChanged(_time);
			}
		}

		void Start()
		{
			if (Actor.HasChildren)
			{
				_previousActor = Actor.GetChildren().DefaultIfEmpty(null).First(actor => actor.IsActive && actor.GetScript<ActorTime>() != null);
			}
			/*foreach (Actor actor in Actor.GetChildren())
			{
				actor.IsActive = false;
			}

			Settings settings = FindScriptInParents<Settings>(Actor);
			if (settings)
			{
				Time = settings.DefaultTime;
			}
			else
			{
				Time = SDateTime.Zero;
			}*/
		}

		//Bad idea
		private T FindScriptInParents<T>(Actor actor) where T : Script
		{
			while (actor != null)
			{
				T script = actor.GetScript<T>();
				if (script) return script;

				actor = actor.Parent;
			}
			return null;
		}

		private void TimeChanged(SDateTime time)
		{
			//Can be removed
			if (!SceneManager.IsGameLogicRunning || Actor == null) return;

			if (Actor.ChildCount <= 0) return;

			Actor toActivate = null;
			long shortestDuration = long.MaxValue;
			for (int i = 0; i < Actor.ChildCount; i++)
			{
				ActorTime actorTime = Actor.GetChild(i).GetScript<ActorTime>();
				if (actorTime)
				{
					long duration = Math.Abs(time.Ticks - actorTime.Time.Ticks);
					if (duration < shortestDuration)
					{
						shortestDuration = duration;
						toActivate = Actor.GetChild(i);
					}

					//Special case: duration can be long.MaxValue which means that it wouldn't be smaller than shortestDuration 
					if (duration == shortestDuration && toActivate == null)
					{
						toActivate = Actor.GetChild(i);
					}
				}
			}

			if (_previousActor == toActivate) return;

			if (toActivate)
			{
				if (_previousActor) _previousActor.IsActive = false;

				toActivate.IsActive = true;
			}
			_previousActor = toActivate;
		}
	}
}
