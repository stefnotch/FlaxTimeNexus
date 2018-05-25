using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

					foreach (Actor child in actor.GetChildren())
					{
						s.Push(child);
					}
				}
			}
		}

	}
}
