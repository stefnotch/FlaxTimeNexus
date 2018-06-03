using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Rendering;
using FlaxEngine.Utilities;

namespace Test
{
	public class CamRay : Script
	{
        public Camera myCam;

		private void Update()
		{
            RayCastHit hit;
            Vector2 mousePos = Input.MousePosition;
            Ray myRay = ConvertMouseToRay(ref mousePos);
			DebugDraw.DrawLine(myRay.Position, myRay.Position + myRay.Direction * 100f, Color.White);

			if (Physics.RayCast(myRay.Position, myRay.Direction, out hit))
            {
                DebugDraw.DrawSphere(hit.Point, 10, Color.Red);
            }

        }

        public Ray ConvertMouseToRay(ref Vector2 mousePosition)
        {
            float width = MainRenderTask.Instance.Buffers.Size.X;
            float height = MainRenderTask.Instance.Buffers.Size.Y;
            // Prepare
            var viewport = new FlaxEngine.Viewport(0, 0, width, height);
            Matrix v, p, ivp;
            CreateProjectionMatrix(out p);
            CreateViewMatrix(out v);

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
        protected virtual void CreateProjectionMatrix(out Matrix result)
        {
            float width = MainRenderTask.Instance.Buffers.Size.X;
            float height = MainRenderTask.Instance.Buffers.Size.Y;
            float aspect = width / height;
            Matrix.PerspectiveFovLH(myCam.FieldOfView * Mathf.DegreesToRadians, aspect, myCam.NearPlane, myCam.FarPlane, out result);
        }

        /// <summary>
        /// Creates the view matrix.
        /// </summary>
        /// <param name="result">The result.</param>
        protected virtual void CreateViewMatrix(out Matrix result)
        {
            // Create view matrix
            Vector3 position = myCam.Position;
            Vector3 direction = myCam.Direction;
            Vector3 target = position + direction;
            Vector3 right = Vector3.Normalize(Vector3.Cross(Vector3.Up, direction));
            Vector3 up = Vector3.Normalize(Vector3.Cross(direction, right));
            Matrix.LookAtLH(ref position, ref target, ref up, out result);
        }
    }
}
