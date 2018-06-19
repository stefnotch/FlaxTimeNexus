using FlaxEngine;

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
		private InputAxis MouseX = new InputAxis("Mouse X");
		private InputAxis MouseY = new InputAxis("Mouse Y");
		private InputAxis VerticalAxis = new InputAxis("Vertical");
		private InputAxis HorizontalAxis = new InputAxis("Horizontal");
		private InputAxis Sprint = new InputAxis("Sprint");
		private float _pitch;
		private float _yaw;
		private bool _jump;
		//private Vector3 _prevVelocity = Vector3.Zero;
		private EdgeTilt _edgeTilt;
		private Vector3 _smoothEdgeTilt;
		private float _edgeTiltSmoothing = 0.1f;

		private void Start()
		{
			_edgeTilt = new EdgeTilt(Player, new Vector3(0, -Player.Height / 2f - Player.Radius, 0));
			Vector3 eulerAngles = Camera.Orientation.EulerAngles;
			_pitch = eulerAngles.X;
			_yaw = eulerAngles.Y;
		}

		//Until I think up something better
		//TODO: Think up a neater solution
		public void SetOrientation(Quaternion orientation)
		{
			Vector3 eulerAngles = orientation.EulerAngles;
			_pitch = eulerAngles.X;
			_yaw = eulerAngles.Y;
		}

		public void SetPitch(Quaternion orientation)
		{
			Vector3 eulerAngles = orientation.EulerAngles;
			_pitch = eulerAngles.X;
		}

		private void Update()
		{
			//The camera may move while the player is paused
			Screen.CursorVisible = false;
			Screen.CursorLock = CursorLockMode.Locked;

			Vector2 mouseDelta = new Vector2(MouseX.Value, MouseY.Value);

			//TODO: Change _pitch and _yaw when the player gets TPd
			_pitch = Mathf.Clamp(_pitch + mouseDelta.Y, -88, 88);
			_yaw += mouseDelta.X;

			//However, the player shouldn't be able to run around
			if (Time.TimeScale == 0) return;

			// Jump
			if (Input.GetAction("Jump"))
			{
				_jump = true;
			}
		}

		private void FixedUpdate()
		{
			// Camera update
			var camFactor = Mathf.Clamp01(CameraSmoothing * Time.UnscaledDeltaTime);
			Vector3 angleOffsets = _edgeTilt.GetAngles() * 0.0f;
			_smoothEdgeTilt = Vector3.SmoothStep(_smoothEdgeTilt, angleOffsets, _edgeTiltSmoothing * Time.UnscaledDeltaTime);
			Player.LocalOrientation = Quaternion.Lerp(Player.LocalOrientation, Quaternion.Euler(0, _yaw, 0), camFactor);
			Camera.LocalOrientation = Quaternion.Lerp(Camera.LocalOrientation, Quaternion.Euler(_pitch - angleOffsets.X, 0, angleOffsets.Z), camFactor);

			// Direction
			var velocity = new Vector3(HorizontalAxis.Value, 0.0f, VerticalAxis.Value);
			velocity.Normalize();
			velocity = Camera.Transform.TransformDirection(velocity);

			velocity.Y = 0;
			velocity.Normalize();

			if (velocity.Length < 0.05f) velocity = Vector3.Zero;


			Vector3 horizontalPrevVelocity = new Vector3(Player.Velocity.X, 0, Player.Velocity.Z);

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
				velocity.Y = Player.Velocity.Y;
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
					velocity.Y *= 0.5f * Time.DeltaTime;
				}
			}

			// Move
			Player.Move(velocity * Time.DeltaTime);
			Player.Velocity = velocity;
		}
	}
}

