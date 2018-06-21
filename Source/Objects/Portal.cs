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
			_task.Camera = RenderTargetCamera;
			_task.Output = _output;


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
				task.Camera.Position = positionDelta /*+ _startTranslation*/ + task.Camera.Parent.Position; //TODO: Dafug, scene offset!!
				task.Camera.Orientation = Camera.MainCamera.Orientation;
				task.Camera.CustomAspectRatio = Camera.MainCamera.Viewport.AspectRatio; //TODO: Whay doe? Shouldn't they always be equal?
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
	}
}
