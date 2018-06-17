using FlaxEngine;
using System;

namespace FlaxTimeNexus
{
	public class OnLevelLoad : Script
	{
		private void Start()
		{
			SceneManager.SceneLoaded += SceneManager_SceneLoaded;
		}

		private void SceneManager_SceneLoaded(Scene scene, Guid sceneId)
		{
			//Only for non-startup scenes
			if (SceneManager.LoadedScenesCount <= 1) return;

			//Transform transform;
			for (int i = 0; i < scene.ChildCount; i++)
			{
				Actor child = scene.GetChild(i);
				if (child.Name != "SceneActor")
				{
					Destroy(ref child); //Flax specific way of destroying objects
				}
				else
				{
					LevelConnector levelConnector = child.GetScriptInChildren<LevelConnector>(); //TODO: Get one that !IsExit (and is disabled)
					if (levelConnector && LevelConnector.LevelsToLoad.TryGetValue(sceneId, out LevelConnector otherLevelConnector))
					{
						levelConnector.Other = otherLevelConnector;
						otherLevelConnector.Other = levelConnector;

						Quaternion desiredRotation = Quaternion.RotationY(Mathf.Pi) * otherLevelConnector.Transform.Orientation;
						child.Orientation = Quaternion.Invert(levelConnector.Actor.LocalOrientation) * desiredRotation; //TODO: Is LocalOrientation the correct one or is it Orientation?
						child.Position = otherLevelConnector.Actor.Position - levelConnector.Actor.Position;


						levelConnector.Actor.IsActive = true;

						LevelConnector.LevelsToLoad.Remove(sceneId);
					}
				}
			}
		}


		private void OnDestroy()
		{
			//Don't ever forget to destroy the event things!!
			SceneManager.SceneLoaded -= SceneManager_SceneLoaded;
		}
	}
}
