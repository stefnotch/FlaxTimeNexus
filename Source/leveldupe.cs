using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus.Source
{
	public class leveldupe : Script
	{
		private void Update()
		{
			if (Input.GetKeyUp(Keys.C))
			{
				Actor clone = CloneActor(this.Actor.FindActor("SceneActor"));
				clone.Position = clone.Position - Vector3.UnitX * 1100f;

				
				//SceneManager.IsGameLogicRunning
				//SceneManager.SaveSceneToBytes()

				//SceneToClone
			}
			//Scene.
		}

		private Actor CloneActor(Actor actor)
		{
			if (actor is Scene) throw new ArgumentException("Scenes can't be cloned that easily");

			List<Actor> actorsToClone = new List<Actor>();
			actorsToClone.AddRange(actor.DepthFirst());

			byte[] actorBytes = Actor.ToBytes(actorsToClone.ToArray());

			Dictionary<Guid, Guid> guidsRemapping = new Dictionary<Guid, Guid>();
			foreach (Guid guid in Actor.TryGetSerializedObjectsIds(actorBytes))
			{
				guidsRemapping[guid] = Guid.NewGuid();
			}

			Actor[] clonedActors = Actor.FromBytes(actorBytes, guidsRemapping);

			return clonedActors.First(a => a.ID == guidsRemapping[actor.ID]);
		}
	}
}
