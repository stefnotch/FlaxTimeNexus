using FlaxEngine;
using FlaxEngine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTemplate
{
	public class TimeGun : Script
	{
		public int TimeIncrement = 1;
		public Model GunBeamModel;

		/// <summary>
		/// After how many seconds should the gun beam disappear
		/// </summary>
		public float FadeTime = 1f;

		EmptyActor GunBeamContainer;
		ModelActor GunBeam;

		readonly InputAxis TimeScroll = new InputAxis("Time");
		readonly float MaxDistance = 100 * 100;

		float _lastFireTime;
		MaterialParameter _materialLength;
		RayCastHit _lastTarget;

		void Start()
		{
			if (this.Actor.GetChild("GunBeam"))
			{
				Destroy(this.Actor.GetChild("GunBeam"));
			}
			//Create a gun beam (container)
			GunBeamContainer = EmptyActor.New();
			GunBeamContainer.Name = "GunBeam";
			GunBeamContainer.IsActive = false;
			//Create the actual gun beam
			GunBeam = ModelActor.New();
			GunBeam.Model = GunBeamModel;
			GunBeam.Scale = Vector3.One * 0.1f;

			//Attach them to the gun
			GunBeamContainer.AddChild(GunBeam, false);
			this.Actor.AddChild(GunBeamContainer, false);

			//Rotate the actual gun beam!
			GunBeam.LocalOrientation = Quaternion.Euler(90, 0, 0);
			GunBeam.LocalPosition = Vector3.Zero;

			GunBeamContainer.LocalPosition = Vector3.Zero;

			_lastFireTime = -FadeTime - 1;

			_materialLength = GunBeam.GetMaterial(0).GetParam("Length");
		}

		void Update()
		{
			RayCastHit[] hits = Physics.RayCastAll(Camera.MainCamera.Position, Camera.MainCamera.Direction, MaxDistance/*, this.Actor.Layer*/);

			Array.Sort(hits, (a, b) => (int)((a.Distance - b.Distance) * 10));

			if (hits.Length > 1)
			{
				RayCastHit hit = hits[0].Collider.Tag == "Player"? hits[1] : hits[0];

				DebugDraw.DrawSphere(hit.Point, 33, Color.Red);

				Collider collider = hit.Collider;
				//TODO: How far up in the hierarchy should we look?
				TimeContainer timeContainer = hit.Collider.GetScript<TimeContainer>();
				if (timeContainer == null)
				{
					if (hit.Collider.Parent)
					{
						timeContainer = hit.Collider.Parent.GetScript<TimeContainer>();
					}
				}

				if (timeContainer == null)
				{
					if (hit.Collider.Parent.Parent)
					{
						timeContainer = hit.Collider.Parent.Parent.GetScript<TimeContainer>();
					}
				}
				if (timeContainer && TimeScroll.Value != 0)
				{
					int scroll = 0;
					if (TimeScroll.Value > 0)
					{
						scroll = Mathf.CeilToInt(TimeScroll.Value);
					}
					else
					{
						scroll = Mathf.FloorToInt(TimeScroll.Value);
					}
					TimeSpan test = TimeSpan.FromDays(365.242 * scroll * TimeIncrement);
					timeContainer.Time = SafeAdd(timeContainer.Time, TimeSpan.FromDays(365.242 * scroll * TimeIncrement));

					UpdateBeam(hit);
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

		void OnDestroy()
		{
			Destroy(GunBeam);
			Destroy(GunBeamContainer);
		}

		private DateTime SafeAdd(DateTime dateTime, TimeSpan timeSpan)
		{
			TimeSpan max = (DateTime.MaxValue - dateTime);
			TimeSpan min = (DateTime.MinValue - dateTime);
			return (min <= timeSpan && timeSpan <= max) ? dateTime.Add(timeSpan) : dateTime;
		}

		private void UpdateBeam(RayCastHit target = default(RayCastHit))
		{
			if (target.Collider)
			{
				GunBeamContainer.IsActive = true;
				_lastFireTime = Time.GameTime;
				_lastTarget = target;
			}

			if ((Time.GameTime - _lastFireTime) < FadeTime)
			{
				if (target.Collider)
				{
					SetBeamLength(target.Distance);
					GunBeamContainer.LookAt(target.Point);
				}
				else
				{
					SetBeamLength(Mathf.Abs((Camera.MainCamera.Position - _lastTarget.Point).Length));
					GunBeamContainer.LookAt(_lastTarget.Point);
				}
			}
			else
			{
				GunBeamContainer.IsActive = false;
			}
		}

		private void SetBeamLength(float beamLength)
		{
			float scaleFactor = GunBeam.Model.Box.Size.Y;

			GunBeam.LocalScale = new Vector3(GunBeam.LocalScale.X, beamLength / scaleFactor, GunBeam.LocalScale.Z);
			GunBeam.LocalPosition = new Vector3(0, 0, beamLength / 2f);
			_materialLength.Value = beamLength / scaleFactor;
		}
	}
}
