using FlaxEngine;
using System;
using System.Linq;

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
		[HideInEditor]
		public Actor CurrentlyActive { get; private set; }

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

		private void Start()
		{
			if (Actor.HasChildren)
			{
				CurrentlyActive = Actor.GetChildren().DefaultIfEmpty(null).FirstOrDefault(actor => actor.IsActive && actor.GetScript<ActorTime>() != null);
				_time = CurrentlyActive?.GetScript<ActorTime>()?.Time ?? SDateTime.Zero;
			}
		}

		[Obsolete("This is quite frequently a bad idea, since Flax doesn't guarantee that the parents are already loaded at the beginning")]
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

		public Actor GetActorByTime(SDateTime time)
		{
			Actor closestActor = null;
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
						closestActor = Actor.GetChild(i);
					}

					//Special case: duration can be long.MaxValue which means that it wouldn't be smaller than shortestDuration 
					if (duration == shortestDuration && closestActor == null)
					{
						closestActor = Actor.GetChild(i);
					}
				}
			}

			return closestActor;
		}

		private void TimeChanged(SDateTime time)
		{
			//Can be removed
			if (!SceneManager.IsGameLogicRunning || Actor == null) return;

			if (Actor.ChildCount <= 0) return;

			var toActivate = GetActorByTime(time);

			if (CurrentlyActive == toActivate) return;

			if (toActivate)
			{
				if (CurrentlyActive) CurrentlyActive.IsActive = false;

				toActivate.IsActive = true;
			}
			CurrentlyActive = toActivate;
		}
	}
}
