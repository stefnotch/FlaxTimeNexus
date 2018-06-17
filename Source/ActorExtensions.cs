using FlaxEngine;
using System;
using System.Collections.Generic;

namespace FlaxTimeNexus
{
	public static class ActorExtensions
	{
		/// <summary>
		/// A depth first traversal of the actor tree
		/// </summary>
		/// <param name="start">Root actor</param>
		/// <param name="keepGoing">Should this actor and its children get returned as well?</param>
		public static IEnumerable<Actor> DepthFirst(this Actor start, Func<Actor, bool> keepGoing = null)
		{
			Stack<Actor> s = new Stack<Actor>();
			s.Push(start);
			while (s.Count > 0)
			{
				Actor actor = s.Pop();
				if (keepGoing == null || keepGoing(actor))
				{
					yield return actor;

					for (int i = 0; i < actor.ChildCount; i++)
					{
						s.Push(actor.GetChild(i));
					}
				}
			}
		}

		/// <summary>
		/// Gets a script in this actor and it's direct children
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="actor"></param>
		/// <returns></returns>
		public static T GetScriptInChildren<T>(this Actor actor) where T : Script
		{
			T script = actor.GetScript<T>();
			if (script != null) return script;

			for (int i = 0; i < actor.ChildCount; i++)
			{
				script = actor.GetChild(i).GetScript<T>();
				if (script != null) return script;
			}

			return null;
		}

	}
}
