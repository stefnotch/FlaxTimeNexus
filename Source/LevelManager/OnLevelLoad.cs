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

			for (int i = 0; i < scene.ChildCount; i++)
			{
				Actor child = scene.GetChild(i);
				if (child.Name != "SceneActor")
				{
					Destroy(ref child); //Flax specific way of destroying objects
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
