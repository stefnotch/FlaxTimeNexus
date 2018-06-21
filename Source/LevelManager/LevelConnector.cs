using FlaxEngine;
using System;
using System.Collections.Generic;

namespace FlaxTimeNexus
{
	/// <summary>
	/// It connects 2 different levels when enabled
	/// </summary>
	public class LevelConnector : Script
	{
		[NoSerialize]
		public static Dictionary<Guid, LevelConnector> LevelsToLoad = new Dictionary<Guid, LevelConnector>();


		/// <summary>
		/// Is this level connector an exit
		/// </summary>
		public bool IsExit = false; //TODO: Or IsEntrance?

		/// <summary>
		/// The level which will get loaded
		/// TODO: Change this to a "random" level!
		/// </summary>
		//[AssetReference(typeof(JSONSceneContainer), true)]
		public string ToLoad;

		/// <summary>
		/// The other <see cref="LevelConnector"/>
		/// </summary>
		[HideInEditor]
		public LevelConnector Other;


		[NoSerialize]
		private bool _isLoaded = false;

		private void OnEnable()
		{
			//Load the next level
			if (!_isLoaded && Other == null && !string.IsNullOrEmpty(ToLoad))
			{
				_isLoaded = true;

				Content.GetAssetInfo(StringUtils.CombinePaths(Globals.ContentFolder, ToLoad), out string typeName, out Guid lvlGuid);


				if (LevelsToLoad.ContainsKey(lvlGuid))
				{
					//TODO: Properly handle this case!
					LevelsToLoad.Remove(lvlGuid);
				}
				LevelsToLoad.Add(lvlGuid, this);

				if (SceneManager.LoadSceneAsync(lvlGuid))
				{
					Debug.LogError("Level loading failed: " + ToLoad);
					LevelsToLoad.Remove(lvlGuid);
				}
			}

		}

		private void OnDisable()
		{
			//TODO: Unload the other level
		}
	}
}
