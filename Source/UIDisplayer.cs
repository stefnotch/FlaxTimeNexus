using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;

namespace FlaxTimeNexus
{
	public class UIDisplayer : Script
	{
		//Create a new material like in this tutorial: https://docs.flaxengine.com/manual/graphics/cameras/render-camera-to-texture.html
		public Material RenderMat;

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
					if (output)
					{
						output.Init(PixelFormat.R8G8B8A8_UNorm, resolution);
					}
				}
			}
		}

		private Vector2 resolution = new Vector2(640, 374);
		private RenderTarget output;
		private CustomRenderTask task;

		private ContainerControl guiRoot;

		private void Start()
		{
			guiRoot = new ContainerControl();
			guiRoot.BackgroundColor = Color.Aqua;
			guiRoot.Bounds = new Rectangle(0, 0, Resolution.X, Resolution.Y);

			Button butt = new Button();
			butt.Text = "Hai";
			butt.BackgroundColor = Color.Red;
			butt.Clicked += () =>
			{
				Debug.Log("Clicked");
			};
			guiRoot.AddChild<Button>(butt);



			// Create backbuffer

			if (output == null)
				output = RenderTarget.New();
			output.Init(PixelFormat.R8G8B8A8_UNorm, resolution);

			// Create rendering task
			if (task == null)
				task = RenderTask.Create<CustomRenderTask>();
			task.OnRender = (gpuContext) =>
			{
				Render2D.CallDrawing(guiRoot, gpuContext, output);
			};

			task.Order = -100;
			task.Enabled = false;
		}

		private void Update()
		{
			
			DebugDraw.DrawSphere(Vector3.Zero, 33, Color.Red);

			task.Enabled = true;
			if (RenderMat) RenderMat.GetParam("Image").Value = output;

			//Well, there is a physics bug right now...so...yeah
			if (Input.GetMouseButtonDown(MouseButton.Left) || Input.GetMouseButtonUp(MouseButton.Left))
			{
				RayCastHit[] hits = Physics.RayCastAll(Camera.MainCamera.Position, Camera.MainCamera.Direction);

				Array.Sort(hits, (a, b) => (int)((a.Distance - b.Distance) * 10));

				RayCastHit hit = default(RayCastHit);
				foreach (RayCastHit h in hits)
				{
					if (h.Collider && h.Collider.Parent == this)
					{
						hit = h;
					}
				}


				if (hit.Collider != null)
				{
					//Or some other point (world coordinates)
					Vector3 hitPos = hit.Point;
					
					
					//Transform it to a local point & rotate it so that it's on a 2D plane
					Vector3 localHitPos = this.Actor.Transform.WorldToLocal(hitPos);
					Vector3 onPlanePos = Vector3.Transform(localHitPos, this.Actor.Orientation);

					

					Vector2 onPlanePos2D = new Vector2(onPlanePos.X, onPlanePos.Y);

					//Normalized device coordinates
					onPlanePos2D /= new Vector2(50); 

					//To GUI-coordinates
					onPlanePos2D += new Vector2(1, -1);
					onPlanePos2D /= new Vector2(2, -2);
					onPlanePos2D *= new Vector2(guiRoot.Width, guiRoot.Height);
					
					//Click
					if (Input.GetMouseButtonDown(MouseButton.Left))
					{
						guiRoot.OnMouseDown(onPlanePos2D, MouseButton.Left);
					}
					else
					{
						guiRoot.OnMouseUp(onPlanePos2D, MouseButton.Left);
					}



				}
			}

			guiRoot.Update(Time.DeltaTime);
		}

		private void OnDisable()
		{
			RenderMat.GetParam("Image").Value = null;
			// Cleanup
			Destroy(ref task);
			Destroy(ref output);
		}
	}
}
