using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.Rendering;

namespace FlaxTimeNexus
{
	public class MatrixSetter : Script
	{
		private MaterialParameter _matrix;

		[Range(0f, 10f)]
		public float Speed = 1f;

		private void Start()
		{
			_matrix = (this.Actor as ModelActor).GetMaterial(0).GetParam("Matrix");
		}

		private void Update()
		{
			_matrix.Value = Matrix.RotationX(Time.GameTime * Speed);
		}
	}
}
