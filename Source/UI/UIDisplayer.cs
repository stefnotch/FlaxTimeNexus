using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxTimeNexus
{
	public class UIDisplayer : Script
	{
		public Material RenderMaterial;

		public Camera Camera;

		public UICreator UICreator;

		[Limit(1, 2000)]
		public Vector2 Resolution
		{
			get { return resolution; }
			set
			{
				value = Vector2.Clamp(value, new Vector2(1), new Vector2(2000));
				if (resolution != value)
				{
					resolution = value;
					if (_output)
					{
						_output.Init(PixelFormat.R8G8B8A8_UNorm, resolution);
					}
					if (_uiRoot != null)
					{
						_uiRoot = UICreator.CreateUI(Resolution);
					}
				}
			}
		}

		private Vector2 resolution = new Vector2(640, 640);

		[NoSerialize]
		private MaterialInstance _renderMaterial;

		[NoSerialize]
		private bool _setMaterial;

		[NoSerialize]
		private RenderTarget _output;

		[NoSerialize]
		private CustomRenderTask _task;

		[NoSerialize]
		private ContainerControl _uiRoot;

		[NoSerialize]
		private bool _lookingAtUI;
		private void OnEnable()
		{
			if (UICreator)
			{
				_uiRoot = UICreator.CreateUI(Resolution);
			}
			else
			{
				_uiRoot = new ContainerControl();
			}

			if (RenderMaterial && _renderMaterial == null)
			{
				_renderMaterial = RenderMaterial.CreateVirtualInstance();
			}

			_setMaterial = true;

			// Create backbuffer
			if (_output == null)
			{
				_output = RenderTarget.New();
			}
			_output.Init(PixelFormat.R8G8B8A8_UNorm, Resolution);
			// Create rendering task
			if (_task == null)
			{
				_task = RenderTask.Create<CustomRenderTask>();
			}
			_task.OnRender = (gpuContext) => Render2D.CallDrawing(_uiRoot, gpuContext, _output);

			_task.Order = 1;
			_task.Enabled = false;
		}

		private void Update()
		{
			if (_setMaterial)
			{
				_task.Enabled = true;
				_setMaterial = false;
				if (_renderMaterial)
				{
					_renderMaterial.GetParam("Image").Value = _output;
				}
				if (Actor is ModelActor modelActor && modelActor.HasContentLoaded)
				{
					modelActor.Entries[0].Material = _renderMaterial;
				}
			}

			Ray ray;
			//if (Screen.CursorLock == CursorLockMode.None)
			//{
			//	Vector2 mousePos = Input.MousePosition;
			//	ray = ConvertMouseToRay(ref mousePos, ref Camera);
			//	DebugDraw.DrawLine(ray.Position, ray.Direction * 400f, Color.Red);
			//}
			//else
			//{
				ray = new Ray(Camera.Position, Camera.Direction);
			//}


			if (Physics.RayCast(ray.Position, ray.Direction, out var hit, layerMask: Camera.Layer) && hit.Collider.Parent == this)
			{
				Vector3 hitPos = hit.Point;
				DebugDraw.DrawSphere(hitPos, 33f, Color.Red);
				//Or some other point (world coordinates)


				//Transform it to a local point & rotate it so that it's on a 2D plane
				Vector3 localHitPos = this.Actor.Transform.WorldToLocal(hitPos);
				Vector3 onPlanePos = Vector3.Transform(localHitPos, this.Actor.LocalOrientation);
				Vector2 onPlanePos2D = new Vector2(onPlanePos.X, onPlanePos.Y);

				//I messed up somewhere. IDK where though. :/

				//Normalized device coordinates
				onPlanePos2D /= new Vector2(50);

				//To GUI-coordinates
				onPlanePos2D += new Vector2(1, -1);
				onPlanePos2D /= new Vector2(2, -2);
				onPlanePos2D *= new Vector2(_uiRoot.Width, _uiRoot.Height);

				if (!_lookingAtUI)
				{
					_uiRoot.OnMouseEnter(onPlanePos2D);
					_lookingAtUI = true;
				}
				else
				{
					_uiRoot.OnMouseMove(onPlanePos2D);
				}
				if (Input.GetMouseButtonDown(MouseButton.Left) || Input.GetMouseButtonUp(MouseButton.Left))
				{
					//Click
					if (Input.GetMouseButtonDown(MouseButton.Left))
					{
						_uiRoot.OnMouseDown(onPlanePos2D, MouseButton.Left);
					}
					else
					{
						//TODO: Improve this, the mouse can go up when the player isn't looking at the UI anymore
						_uiRoot.OnMouseUp(onPlanePos2D, MouseButton.Left);
					}


				}
			}
			else if (_lookingAtUI)
			{
				_uiRoot.OnMouseLeave();
			}

			_uiRoot.Update(Time.DeltaTime);
		}

		private void OnDisable()
		{
			// Cleanup
			Destroy(ref _task);
			Destroy(ref _output);
			Destroy(ref _renderMaterial);

			_uiRoot.OnDestroy();
		}

		public Ray ConvertMouseToRay(ref Vector2 mousePosition, ref Camera camera)
		{
			float width = MainRenderTask.Instance.Buffers.Size.X;
			float height = MainRenderTask.Instance.Buffers.Size.Y;
			// Prepare
			var viewport = new FlaxEngine.Viewport(0, 0, width, height);
			Matrix v, p, ivp;
			CreateProjectionMatrix(out p, ref camera);
			CreateViewMatrix(out v, ref camera);

			Matrix.Multiply(ref v, ref p, out ivp);
			ivp.Invert();

			// Create near and far points
			Vector3 nearPoint = new Vector3(mousePosition, 0.0f);
			Vector3 farPoint = new Vector3(mousePosition, 1.0f);
			viewport.Unproject(ref nearPoint, ref ivp, out nearPoint);
			viewport.Unproject(ref farPoint, ref ivp, out farPoint);

			// Create direction vector
			Vector3 direction = farPoint - nearPoint;
			direction.Normalize();
			Debug.Log(direction);
			return new Ray(nearPoint, direction);
		}

		/// <summary>
		/// Creates the projection matrix.
		/// </summary>
		/// <param name="result">The result.</param>
		protected virtual void CreateProjectionMatrix(out Matrix result, ref Camera camera)
		{
			float width = MainRenderTask.Instance.Buffers.Size.X;
			float height = MainRenderTask.Instance.Buffers.Size.Y;
			float aspect = width / height;
			Matrix.PerspectiveFovLH(camera.FieldOfView * Mathf.DegreesToRadians, aspect, camera.NearPlane, camera.FarPlane, out result);
		}

		/// <summary>
		/// Creates the view matrix.
		/// </summary>
		/// <param name="result">The result.</param>
		protected virtual void CreateViewMatrix(out Matrix result, ref Camera camera)
		{
			// Create view matrix
			Vector3 position = camera.Position;
			Vector3 direction = camera.Direction;
			Vector3 target = position + direction;
			Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.Up, direction));
			Vector3 up = Vector3.Normalize(Vector3.Cross(direction, right));
			Matrix.LookAtLH(ref position, ref target, ref up, out result);
		}

		/*private Ray ConvertMouseToRay(Vector2 mouse, Camera camera)
		{
			//Nope
			Vector3 near = new Vector3(mouse, 0);
			Vector3 far = new Vector3(mouse, 1);
			Viewport vp = new Viewport(Vector2.Zero, MainRenderTask.Instance.Buffers.Size);
			near = vp.Unproject(near, camera.Projection, camera.View, Matrix.Identity);
			far = vp.Unproject(far, camera.Projection, camera.View, Matrix.Identity);

			Vector3 dir = far - near;
			dir.Normalize();
			Debug.Log(dir);
			return new Ray(near, dir);

			Vector4 ndc = new Vector4(
				mouse.X * 2f / MainRenderTask.Instance.Buffers.Size.X - 1f,
				1f - mouse.Y * 2f / MainRenderTask.Instance.Buffers.Size.Y,
				0,
				1
			);

			Vector4 dir = ndc;

			//dir *= new Vector4(MainRenderTask.Instance.Buffers.Size.X / MainRenderTask.Instance.Buffers.Size.Y, 1, 1, 1);
			dir = Vector4.Transform(dir, Matrix.Invert(camera.View * camera.Projection));

			Vector3 d3 = new Vector3(dir.X, dir.Y, dir.Z) / dir.W;
			return new Ray(camera.Position, (d3 - camera.Position).Normalized);
	}*/
	}
}
