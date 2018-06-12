using FlaxEngine;
using FlaxEngine.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace FlaxTimeNexus
{
	//TODO: Rename this to something better
	//TODO: Need some major overhauling, because the code is so horrible
	internal class TransitionTimeContainer
	{
		private class TimeMaterialReplacement
		{
			public readonly MaterialBase OriginalMaterial;
			public readonly MaterialInstance MaterialInstance;
			public readonly MaterialParameter MaterialParameter;
			public ModelEntryInfo ModelEntryInfo;

			public TimeMaterialReplacement(MaterialBase originalMaterial)
			{
				this.OriginalMaterial = originalMaterial;

				string materialName;
				if (originalMaterial is MaterialInstance materialInstance)
				{
					materialName = System.IO.Path.GetFileName(materialInstance.BaseMaterial.Path);
				}
				else
				{
					materialName = System.IO.Path.GetFileName(originalMaterial.Path);
				}
				string path = System.IO.Path.Combine(Globals.ContentFolder, "Materials", "Time Scrolling", "Overrides", materialName);

				MaterialInstance = Content.Load<MaterialBase>(path).CreateVirtualInstance();

				for (int j = 0; j < originalMaterial.Parameters.Length; j++)
				{
					MaterialInstance.Parameters[j].Value = originalMaterial.Parameters[j].Value; //TODO: Is that a good idea?
				}

				MaterialParameter = MaterialInstance.GetParam("T");
			}
		}

		/// <summary>
		/// How long should the time transition take (seconds)
		/// </summary>
		public static readonly float TransitionDuration = 0.5f;


		public TimeContainer TimeContainer { get; }

		public RayCastHit Hit; //Might be useful for the effect in the future
		public SDateTime ToAdd = SDateTime.Zero;

		private SDateTime _previousToAdd;
		private float _transitionValue = 0f;

		public TransitionTimeContainer(TimeContainer timeContainer)
		{
			TimeContainer = timeContainer;
		}

		/// <summary>
		/// Updates the animation
		/// </summary>
		/// <returns>If the animation is done</returns>
		public bool UpdateAnimation()
		{
			//TODO: Go through every single actor or just the beginning and end?
			Actor a = TimeContainer.CurrentlyActive;
			var targetTime = TimeContainer.Time + ToAdd;
			Actor b = TimeContainer.GetActorByTime(targetTime);

			//Should it be a spontaneous time transition
			if (TransitionDuration < Mathf.Epsilon)
			{
				_transitionValue = 1f;
			}
			else
			{

				//If the user scrolled a bit, we have to do some stuff
				if (_previousToAdd == null)//The very beginning
				{
					_transitionValue = 0f;
				}
				else if (ToAdd != _previousToAdd)//Scrolling around a bit after starting
				{

					//Adjust the transition (e.g. t = 0.6, prev = 5 years, curr = 10 years --> t = 0.3)
					_transitionValue *= _previousToAdd.Ticks / (float)ToAdd.Ticks;

					//If he switched "directions" (prev = -5 year, curr = + 10 years) or scrolled back to square one
					if (_transitionValue < 0 || ToAdd.Ticks == 0)
					{
						_transitionValue = 0f;
						//TODO: Shattering effect? (because he "cancelled" the time transition)


					}

				}

				//And, increment the transition value
				_previousToAdd = ToAdd;
				_transitionValue += 1f / (Time.UpdateFPS * TransitionDuration);
			}

			//If the transition is done
			if (_transitionValue >= 1f)
			{
				TimeContainer.Time = targetTime;

				OnComplete();
				return true;
			}

			//Otherwise, actually play a transition effect
			LerpBetweenActors(a, b, _transitionValue);
			return false;
		}

		private void OnComplete()
		{
			foreach (var mat in _timeTransitionMaterials)
			{
				mat.ModelEntryInfo.Material = mat.OriginalMaterial;

				MaterialInstance materialInstance = mat.MaterialInstance;
				Material baseMaterial = mat.MaterialInstance.BaseMaterial;

				Material.Destroy(ref baseMaterial); //Not sure? TODO: Ask someone which things I should dispose of
				MaterialInstance.Destroy(ref materialInstance);
			}
		}

		private readonly List<TimeMaterialReplacement> _timeTransitionMaterials = new List<TimeMaterialReplacement>();
		private Actor _cachedStartActor;
		private Actor _cachedEndActor;
		private bool _cachedEndIsActive;

		/// <summary>
		/// A nice transition effect between 2 actors
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="t">between 0 and 1</param>
		private void LerpBetweenActors(Actor start, Actor end, float t)
		{
			bool startActorChanged = start != _cachedStartActor;
			bool endActorChanged = end != _cachedEndActor;

			if (startActorChanged)
			{
				_cachedStartActor = start;
			}

			//If the end actor is now different, disable the previous one!	
			if (endActorChanged)
			{
				//If the end actor is now different, disable the previous one!
				if (_cachedEndActor != null) _cachedEndActor.IsActive = _cachedEndIsActive;

				_cachedEndIsActive = end.IsActive;
				_cachedEndActor = end;
			}

			if (end == start) return;

			//Caching, TODO: Improve this (better material caching, removal, etc)
			if (startActorChanged)
			{
				foreach (var modelActor in start.DepthFirst().OfType<ModelActor>())
				{
					for (int i = 0; i < modelActor.Entries.Length; i++)
					{
						var originalMaterial = modelActor.GetMaterial(i);

						var materialReplacement = new TimeMaterialReplacement(originalMaterial)
						{
							ModelEntryInfo = modelActor.Entries[i]
						};

						//TODO: End material...
						//newMat.GetParam("Mask Multiplier")

						modelActor.Entries[i].Material = materialReplacement.MaterialInstance;

						_timeTransitionMaterials.Add(materialReplacement);
					}
				}
			}

			if (endActorChanged)
			{
				//TODO: The end actor's triggers and whatnot shouldn't get activated! (Issues can also arise when it's a rigidbody)
				foreach (var modelActor in end.DepthFirst().OfType<ModelActor>())
				{
					for (int i = 0; i < modelActor.Entries.Length; i++)
					{
						var originalMaterial = modelActor.GetMaterial(i);

						var materialReplacement = new TimeMaterialReplacement(originalMaterial)
						{
							ModelEntryInfo = modelActor.Entries[i]
						};

						materialReplacement.MaterialInstance.GetParam("Mask Multiplier").Value = -1;

						modelActor.Entries[i].Material = materialReplacement.MaterialInstance;

						_timeTransitionMaterials.Add(materialReplacement);
					}
				}

				end.IsActive = true;
			}

			//Transition effect
			foreach (var materialReplacement in _timeTransitionMaterials)
			{
				materialReplacement.MaterialParameter.Value = t;
			}

			//TODO: Position lerp effect
			//RigidBody.New().Sleep();
			//Transform.Lerp()
			/*if(t > 0.5)
			{
				start.IsActive = false;
				end.IsActive = true;
			}*/
		}
	}
}