using FlaxEngine;
using FlaxEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	public class TimeGun : Script
	{
		/// <summary>
		/// By how much should the time get incremented
		/// </summary>
		public SDateTime TimeIncrement = SDateTime.Zero;

		/// <summary>
		/// How fast should the time incrementing happen (seconds)
		/// </summary>
		[Tooltip("Speed of the time incrementing in seconds")]
		public float TransitionSpeed = 1f;

		/// <summary>
		/// The model
		/// </summary>
		public Model GunBeamModel;

		/// <summary>
		/// After how many seconds should the gun beam disappear
		/// </summary>
		public float FadeTime = 1f;


		private readonly InputAxis TimeScroll = new InputAxis("Time");
		private readonly float MaxDistance = 100 * 100;
		private readonly string GunBeamName = "GunBeam";
		private EmptyActor _gunBeamContainer;
		private ModelActor _gunBeam;
		private float _lastFireTime;
		private MaterialParameter _materialLength;
		private MaterialParameter _materialDirection;
		private RayCastHit _lastTarget;

		private readonly Dictionary<TimeContainer, TransitionTimeContainer> _timeContainers = new Dictionary<TimeContainer, TransitionTimeContainer>();
		private readonly Dictionary<MaterialBase, MaterialBase> _timeTransitionMaterials = new Dictionary<MaterialBase, MaterialBase>();

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

			//TODO: Change to a camera that has to be set (property)
			if (Physics.RayCast(Camera.MainCamera.Position, Camera.MainCamera.Direction, out RayCastHit hit, MaxDistance, this.Actor.Layer))
			{
				timeContainer = GetTimeContainer(hit.Collider);
				if (timeContainer)
				{
					//TODO: Little effect when something is scrollable
				}
			}


			if (timeContainer != null && TimeScroll.Value != 0)
			{
				//TODO: Shoot the beam()

				if (!_timeContainers.TryGetValue(timeContainer, out var transitionTimeContainer))
				{
					transitionTimeContainer = new TransitionTimeContainer(timeContainer);
				}

				transitionTimeContainer.Hit = hit;
				transitionTimeContainer.ToAdd += TimeScroll.Value * TimeIncrement;
			}
			else
			{
				//TODO: Update the beam()
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
			foreach(var transitionTimeContainer in _timeContainers.Values)
			{
				if (TransitionSpeed < Mathf.Epsilon)
				{
					transitionTimeContainer.TransitionValue = 1f;
				}
				else
				{
					transitionTimeContainer.TransitionValue += 1f / (Time.UpdateFPS * TransitionSpeed);
				}
			}
		}

		private void UpdateBeam(RayCastHit target = default(RayCastHit), float timeScroll = 1)
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
					_materialDirection.Value = timeScroll;
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




		/// <summary>
		/// A nice lerping effect between 2 actors
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="t">between 0 and 1</param>
		private void LerpBetweenActors(Transform start, Transform end, float t)
		{
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
