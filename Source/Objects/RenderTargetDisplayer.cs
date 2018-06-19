using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxTimeNexus.Source.Objects
{
	[ExecuteInEditMode]
	public class RenderTargetDisplayer : Script
	{
		[Tooltip("The first parameter must be the render target image!")]
		public Material RenderTargetMaterial;
		public Camera RenderTargetCamera;

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
			_task.Order = -100;
			_task.Camera = RenderTargetCamera;
			_task.Output = _output;
			_task.Enabled = false;

			if (RenderTargetMaterial && _rtMaterial == null)
			{
				_rtMaterial = RenderTargetMaterial.CreateVirtualInstance();
			}


			setMaterial = true;

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
			Destroy(ref _task);
			Destroy(ref _output);
			Destroy(ref _rtMaterial);
		}

	}
}
