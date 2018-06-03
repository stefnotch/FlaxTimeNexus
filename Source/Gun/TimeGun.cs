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
		/// By how many years should the time get incremented
		/// TODO: Years, months, days, hours, ... (Use noda-time?)
		/// </summary>
		public int TimeIncrement = 1;

		/// <summary>
		/// The model
		/// </summary>
		public Model GunBeamModel;

		/// <summary>
		/// After how many seconds should the gun beam disappear
		/// </summary>
		public float FadeTime = 1f;

		readonly InputAxis TimeScroll = new InputAxis("Time");
		readonly float MaxDistance = 100 * 100;
		readonly string GunBeamName = "GunBeam";

		EmptyActor _gunBeamContainer;
		ModelActor _gunBeam;

		float _lastFireTime;
		MaterialParameter _materialLength;
		MaterialParameter _materialDirection;
		RayCastHit _lastTarget;



		TimeSpan _toAdd = TimeSpan.Zero;

		void Start()
		{
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

		void Update()
		{
			//TODO: Change to a camera that has to be set (property)
			if (Physics.RayCast(Camera.MainCamera.Position, Camera.MainCamera.Direction, out RayCastHit hit, MaxDistance, this.Actor.Layer))
			{
				TimeContainer timeContainer = GetTimeContainer(hit.Collider);

				//TODO: Little effect when something is scrollable

				if (timeContainer && TimeScroll.Value != 0)
				{
					_toAdd = TimeSpan.FromDays(365.242 * TimeScroll.Value * TimeIncrement);
					timeContainer.Time = SafeAdd(timeContainer.Time, _toAdd);

					UpdateBeam(hit, TimeScroll.Value);
				}
				else
				{
					UpdateBeam();
				}

			}
			else
			{
				UpdateBeam();
			}
		}

		private TimeContainer GetTimeContainer(Actor actor)
		{
			//TODO: How far up the hierarchy should we look??
			return actor.GetScript<TimeContainer>() ??
				actor.Parent.GetScript<TimeContainer>() ??
				actor.Parent.Parent.GetScript<TimeContainer>(); //TimeContainer --> Model --> Collider (ILookatTrigger)
		}

		void OnDestroy()
		{
			Destroy(_gunBeam);
			Destroy(_gunBeamContainer);
		}

		private DateTime SafeAdd(DateTime dateTime, TimeSpan timeSpan)
		{
			TimeSpan max = (DateTime.MaxValue - dateTime);
			TimeSpan min = (DateTime.MinValue - dateTime);
			return (min <= timeSpan && timeSpan <= max) ? dateTime.Add(timeSpan) : dateTime;
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
