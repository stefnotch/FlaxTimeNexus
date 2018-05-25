using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	public class FPSCamera : Script
	{
		public float Acceleration = 10000f;

		[Limit(0, 1)]
		public float Friction = 0.1f;

		public float Speed = 500.0f;
		public float SprintSpeedIncrease = 0.5f;
		public float JumpForce = 500f;

		public CharacterController Player;
		public Camera Camera;
		public float CameraSmoothing = 20.0f;

		InputAxis MouseX = new InputAxis("Mouse X");
		InputAxis MouseY = new InputAxis("Mouse Y");

		InputAxis VerticalAxis = new InputAxis("Vertical");
		InputAxis HorizontalAxis = new InputAxis("Horizontal");

		InputAxis Sprint = new InputAxis("Sprint");

		float _pitch;
		float _yaw;

		bool _jump;

		Vector3 _prevVelocity = Vector3.Zero;

		EdgeTilt _edgeTilt;
		Vector3 _smoothEdgeTilt;
		float _edgeTiltSmoothing = 0.1f;

		void Start()
		{
			_edgeTilt = new EdgeTilt(Player, new Vector3(0, -Player.Height / 2f - Player.Radius, 0));
		}

		void Update()
		{
			if (Time.TimeScale == 0) return;

			Screen.CursorVisible = false;
			Screen.CursorLock = CursorLockMode.Locked;

			Vector2 mouseDelta = new Vector2(MouseX.Value, MouseY.Value);
			_pitch = Mathf.Clamp(_pitch + mouseDelta.Y, -88, 88);
			_yaw += mouseDelta.X;

			// Jump
			if (Input.GetAction("Jump"))
			{
				_jump = true;
			}
		}

		void FixedUpdate()
		{
			// Camera update
			var camFactor = Mathf.Clamp01(CameraSmoothing * Time.DeltaTime);
			Vector3 angleOffsets = _edgeTilt.GetAngles() * 0.1f;
			_smoothEdgeTilt = Vector3.SmoothStep(_smoothEdgeTilt, angleOffsets, _edgeTiltSmoothing * Time.DeltaTime);
			Player.LocalOrientation = Quaternion.Lerp(Player.LocalOrientation, Quaternion.Euler(0, _yaw, 0), camFactor);
			Camera.LocalOrientation = Quaternion.Lerp(Camera.LocalOrientation, Quaternion.Euler(_pitch - angleOffsets.X, 0, angleOffsets.Z), camFactor);
			
			// Direction
			var velocity = new Vector3(HorizontalAxis.Value, 0.0f, VerticalAxis.Value);
			velocity.Normalize();
			velocity = Camera.Transform.TransformDirection(velocity);

			velocity.Y = 0;
			velocity.Normalize();

			if (velocity.Length < 0.05f) velocity = Vector3.Zero;


			Vector3 horizontalPrevVelocity = new Vector3(_prevVelocity.X, 0, _prevVelocity.Z);

			//TODO: Actually, you can't accelerate in the air.
			//TODO: Friction doesn't respect the Time.DeltaTime
			velocity = (horizontalPrevVelocity * (1 - Friction) + velocity * Acceleration * Time.DeltaTime);


			if (velocity.Length > 0.05f)
			{
				float newSpeed = velocity.Length;
				float maxSpeed = Speed * (Sprint.Value * SprintSpeedIncrease + 1);
				if (newSpeed > maxSpeed)
				{
					velocity *= (maxSpeed / newSpeed);
				}
			}

			// Y-Axis stuff
			if (Player.IsGrounded)
			{
				velocity.Y = -Mathf.Abs(Physics.Gravity.Y * 0.5f);
			}
			else
			{
				velocity.Y = _prevVelocity.Y;
			}

			// Jump
			if (_jump && Player.IsGrounded)
			{
				velocity.Y = JumpForce;
			}
			_jump = false;


			// Apply gravity
			velocity.Y -= Mathf.Abs(Physics.Gravity.Y) * Time.DeltaTime;

			// player head hit something above
			if ((Player.Flags & CharacterController.CollisionFlags.Above) != 0)
			{
				if (velocity.Y > 0)
				{
					velocity.Y = 0;
				}
			}

			// Move
			Player.Move(velocity * Time.DeltaTime);
			_prevVelocity = velocity;
		}
	}
}

