using FlaxEngine;
using System;

namespace FlaxTimeNexus
{
	/// <summary>
	/// Can be added to a trigger
	/// </summary>
	public class LevelLoader : Script
	{
		//[AssetReference(typeof(JSONSceneContainer), true)]
		public string ToLoad;

		private bool _isLoading = false;
		private void Start()
		{
			// Here you can add code that needs to be called when script is created
		}

		private void OnTriggerEnter(Collider c)
		{
			if (!_isLoading)
			{
				_isLoading = true;


				//ToLoad = @"C:\Users/Stefnotch/Documents/GitHub/Games/FlaxTimeNexus/Content/Scenes/Labyrinth.scene";
				Content.GetAssetInfo(StringUtils.CombinePaths(Globals.ContentFolder, ToLoad), out string typeName, out Guid lvlGuid);


				/*GUIDConverterTemp.ParseID("35fd20cd4497ea1b18cafcae1c3524ef", out Guid lvlGuid);*/
				if (SceneManager.LoadSceneAsync(lvlGuid))
				{
					Debug.Log("fail?");
				}
				else
				{
					Debug.Log("yay?");
				}



			}
		}
	}
}
