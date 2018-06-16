using FlaxEngine;
using FlaxEngine.Rendering;
using System.Collections.Generic;

namespace FlaxTimeNexus
{
	public class TimeGun : Script
	{
		/// <summary>
		/// By how much should the time get incremented
		/// </summary>
		public SDateTime TimeIncrement = SDateTime.Zero;

		/// <summary>
		/// The model
		/// </summary>
		public Model GunBeamModel;

		/// <summary>
		/// After how many seconds should the gun beam disappear
		/// </summary>
		public float FadeTime = 1f;

		/// <summary>
		/// Maximum distance
		/// </summary>
		public float MaxDistance = 100 * 100;


		private readonly InputAxis TimeScroll = new InputAxis("Time");
		private readonly string GunBeamName = "GunBeam";
		private EmptyActor _gunBeamContainer;
		private ModelActor _gunBeam;
		private float _lastFireTime;
		private MaterialParameter _materialLength;
		private MaterialParameter _materialDirection;
		private RayCastHit _lastTarget;

		[NoSerialize]
		private readonly Dictionary<TimeContainer, TransitionTimeContainer> _timeContainers = new Dictionary<TimeContainer, TransitionTimeContainer>();

		private void Start()
		{
			//_gunBeam.GetMaterial(0)
			//_gunBeam.Entries[0].Material = 
			if (this.Actor.GetChild(GunBeamName))
			{
				Destroy(this.Actor.GetChild(GunBeamName));
			}
			//Create a gun beam (container)
			_gunBeamContainer = EmptyActor.New();
			_gunBeamContainer.Name = GunBeamName;
			_gunBeamContainer.IsActive = false;
			//Create the actual gun beam
			_gunBeam = ModelActor.New();
			_gunBeam.Model = GunBeamModel;
			_gunBeam.Scale = Vector3.One * 0.1f;

			//Attach them to the gun
			_gunBeamContainer.AddChild(_gunBeam, false);
			this.Actor.AddChild(_gunBeamContainer, false);

			//Rotate the actual gun beam!
			_gunBeam.LocalOrientation = Quaternion.Euler(90, 0, 0);
			_gunBeam.LocalPosition = Vector3.Zero;

			_gunBeamContainer.LocalPosition = Vector3.Zero;

			_lastFireTime = -FadeTime - 1;

			_materialLength = _gunBeam.GetMaterial(0).GetParam("Length");
			_materialDirection = _gunBeam.GetMaterial(0).GetParam("Direction");
		}

		private void Update()
		{
			//Can be null
			TimeContainer timeContainer = null;

			//TODO: Change to a camera that has to be set (property), or use PlayerLookat
			if (Physics.RayCast(Camera.MainCamera.Position, Camera.MainCamera.Direction, out RayCastHit hit, MaxDistance, this.Actor.Layer, hitTriggers: false))
			{
				var beamReflector = hit.Collider.GetScript<BeamReflector>();
				if (beamReflector != null)
				{
					//TODO: Implement it
				}

				timeContainer = GetTimeContainer(hit.Collider);
				if (timeContainer != null)
				{
					//TODO: Little effect when something is scrollable
				}
			}

			//If the player scrolled & is looking at something scrollable
			if (TimeScroll.Value != 0 && timeContainer != null)
			{
				UpdateBeam(hit, TimeScroll.Value);

				if (!_timeContainers.TryGetValue(timeContainer, out var transitionTimeContainer))
				{
					transitionTimeContainer = new TransitionTimeContainer(timeContainer);
					_timeContainers.Add(timeContainer, transitionTimeContainer);
				}

				transitionTimeContainer.Hit = hit;
				transitionTimeContainer.ToAdd += TimeScroll.Value * TimeIncrement;
			}
			else
			{
				UpdateBeam();
			}

			UpdateAnimations();
		}

		private TimeContainer GetTimeContainer(Actor actor)
		{
			//TODO: How far up the hierarchy should we look??
			return actor.GetScript<TimeContainer>() ??
				actor.Parent.GetScript<TimeContainer>() ??
				actor.Parent.Parent.GetScript<TimeContainer>(); //TimeContainer --> Model --> Collider (ILookatTrigger)
		}

		private void OnDestroy()
		{
			Destroy(_gunBeam);
			Destroy(_gunBeamContainer);
		}

		private void UpdateAnimations()
		{
			List<TimeContainer> toRemove = new List<TimeContainer>();
			foreach (var element in _timeContainers)
			{
				if (element.Value.UpdateAnimation())
				{
					toRemove.Add(element.Key);
				}
			}

			foreach (var key in toRemove)
			{
				_timeContainers.Remove(key);
			}
		}

		private void UpdateBeam(RayCastHit target = default(RayCastHit), float scrollSpeed = 1)
		{
			bool hitTarget = (target.Collider != null);
			if (hitTarget)
			{
				_gunBeamContainer.IsActive = true;
				_lastFireTime = Time.GameTime;
				_lastTarget = target;
			}

			if ((Time.GameTime - _lastFireTime) < FadeTime)
			{
				if (hitTarget)
				{
					SetBeamLength(target.Distance);
					_materialDirection.Value = scrollSpeed;
					_gunBeamContainer.LookAt(target.Point);
				}
				else
				{
					SetBeamLength(Mathf.Abs((_gunBeamContainer.Position - _lastTarget.Point).Length));
					_gunBeamContainer.LookAt(_lastTarget.Point);
				}
			}
			else
			{
				_gunBeamContainer.IsActive = false;
			}
		}

		private void SetBeamLength(float beamLength)
		{
			float scaleFactor = _gunBeam.Model.Box.Size.Y;

			_gunBeam.LocalScale = new Vector3(_gunBeam.LocalScale.X, beamLength / scaleFactor, _gunBeam.LocalScale.Z);
			_gunBeam.LocalPosition = new Vector3(0, 0, beamLength / 2f);
			_materialLength.Value = beamLength / scaleFactor;
		}
	}
}
