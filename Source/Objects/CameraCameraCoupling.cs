using FlaxEngine;

namespace FlaxTimeNexus
{
	public class CameraCameraCoupling : Script
	{
		public Actor PlayerCamera;
		public Actor Camera;
		public Actor Base;

		private Vector3 _startTranslation; //TODO: Scene moving --> this breaks
		private void Start()
		{
			_startTranslation = Camera.Position;
		}

		private void Update()
		{
			Vector3 positionDelta = (PlayerCamera.Position - Base.Position);

			Camera.Position = positionDelta + _startTranslation;
			Camera.Orientation = PlayerCamera.Orientation;
			//Camera.Direction = PlayerCamera.Direction;
		}
	}
}
