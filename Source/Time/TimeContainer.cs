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
		private DateTime _time = new DateTime();

		[NoSerialize]
		private Actor _previousActor;

		//TODO: Switch to https://nodatime.org/
		[HideInEditor]
		[NoSerialize]
		public DateTime Time
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
				_previousActor = Actor.GetChildren().First(actor => actor.IsActive && actor.GetScript<ActorTime>() != null);
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
				Time = new DateTime();
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

		private void TimeChanged(DateTime time)
		{
			//Can be removed
			if (!SceneManager.IsGameLogicRunning || Actor == null) return;

			Actor toActivate = null;
			TimeSpan minDuration = TimeSpan.MaxValue;
			foreach (Actor actor in Actor.GetChildren())
			{
				ActorTime actorTime = actor.GetScript<ActorTime>();
				if (actorTime)
				{
					TimeSpan duration = (time - actorTime.Time).Duration();
					if (duration < minDuration)
					{
						toActivate = actor;
						minDuration = duration;
					}
				}
			}
			if (_previousActor == toActivate) return;

			if (_previousActor)
			{
				_previousActor.IsActive = false;
			}

			if (toActivate)
			{
				toActivate.IsActive = true;
			}
			_previousActor = toActivate;
		}
	}
}
