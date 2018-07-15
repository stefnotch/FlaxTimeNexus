using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxTimeNexus.Source.Objects
{
	public class Portal : Script
	{
		[Tooltip("The first parameter must be the render target image!")]
		public Material RenderTargetMaterial;

		[Limit(1, 2000)]
		public Vector2 Resolution
		{
			get { return _resolution; }
			set
			{
				value = Vector2.Clamp(value, new Vector2(1), new Vector2(2000));
				if (_resolution != value)
				{
					_resolution = value;
					if (_output)
					{
						_output.Init(PixelFormat.R8G8B8A8_UNorm, _resolution);
					}
				}
			}
		}

		public Camera RenderTargetCamera
		{
			get => _renderTargetCamera;
			set
			{
				_renderTargetCamera = value;
				if (_task) _task.Camera = _renderTargetCamera;
				_startTranslation = RenderTargetCamera.LocalPosition;
			}
		}

		//public Camera PlayerCamera { get; set; }

		private Vector3 _startTranslation = Vector3.Zero;
		private Camera _renderTargetCamera;
		private MaterialInstance _rtMaterial;
		private RenderTarget _output;
		private Vector2 _resolution = new Vector2(100, 100);
		private SceneRenderTask _task;

		private void OnEnable()
		{
			// Create backbuffer
			if (_output == null)
			{
				_output = RenderTarget.New();
			}
			_output.Init(PixelFormat.R8G8B8A8_UNorm, Resolution); //TODO: Should I just adjust the resolution to the object's size or something?

			// Create rendering task
			if (_task == null)
			{
				_task = RenderTask.Create<SceneRenderTask>();
			}
			_task.Enabled = false;

			_task.Begin += RenderTaskBegin;
			_task.Order = -100;
			//_task.Camera = RenderTargetCamera;
			_task.Output = _output;
			_task.View.CopyFrom(RenderTargetCamera);

			if (RenderTargetMaterial && _rtMaterial == null)
			{
				_rtMaterial = RenderTargetMaterial.CreateVirtualInstance();
			}

			_startTranslation = RenderTargetCamera.LocalPosition;

			setMaterial = true;

		}

		private void RenderTaskBegin(SceneRenderTask task, GPUContext context)
		{
			if (Camera.MainCamera != null && Camera.MainCamera != task.Camera)
			{
				Vector3 positionDelta = (Camera.MainCamera.Position - this.Actor.Position);
				//task.View.View = Camera.MainCamera.View;
				//ToObliqueMatrix(ref )
				//task.View.Projection = 

				//task.Camera.Position = positionDelta /*+ _startTranslation*/ + task.Camera.Parent.Position; //TODO: Dafug, scene offset!!
				//task.Camera.Orientation = Camera.MainCamera.Orientation;
				//task.Camera.CustomAspectRatio = Camera.MainCamera.Viewport.AspectRatio; //TODO: Whay doe? Shouldn't they always be equal?
			}
		}

		private bool setMaterial;

		private void Update()
		{
			if (setMaterial)
			{
				_task.Enabled = true;
				setMaterial = false;

				if (_rtMaterial != null)
				{
					_rtMaterial.GetParam(0).Value = _output;
				}

				if (Actor is ModelActor modelActor)
				{
					if (modelActor.HasContentLoaded)
					{
						modelActor.Entries[0].Material = _rtMaterial;
					}
				}
			}
		}

		private void OnDisable()
		{
			if (_task != null)
			{
				_task.Begin -= RenderTaskBegin;
			}
			Destroy(ref _task);
			Destroy(ref _output);
			Destroy(ref _rtMaterial);
		}

		/// <summary>
		/// http://aras-p.info/texts/obliqueortho.html
		/// </summary>
		/// <param name="projection">Projection matrix which will get modified</param>
		/// <param name="cameraSpaceClipPlane">in camera space</param>
		private void ToObliqueMatrix(ref Matrix projection, Vector4 cameraSpaceClipPlane)
		{
			Vector4 q = Vector4.Transform(
				new Vector4(
					Mathf.Sign(cameraSpaceClipPlane.X),
					Mathf.Sign(cameraSpaceClipPlane.Y),
					1.0f,
					1.0f), Matrix.Invert(projection));

			Vector4 c = cameraSpaceClipPlane * (2.0f / (Vector4.Dot(cameraSpaceClipPlane, q)));
			// third row = clip plane - fourth row
			projection[2] = c.X - projection[3];
			projection[6] = c.Y - projection[7];
			projection[10] = c.Z - projection[11];
			projection[14] = c.W - projection[15];
		}
	}
}
