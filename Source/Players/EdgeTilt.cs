using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTemplate
{
	public class EdgeTilt
	{
		const int RayCount = 16;
		float _radius = 100.0f;
		Vector3[] _offsets = new Vector3[RayCount];

		CharacterController _player;
		Vector3 _feetOffset;

		public EdgeTilt(CharacterController player, Vector3 feetOffset)
		{
			_player = player;
			_feetOffset = feetOffset;

			for (int i = 0; i < RayCount; i++)
			{
				// Offset Vector, from 0 to some place on a circle
				_offsets[i] = (Vector3.UnitZ * _radius) * Quaternion.RotationY(i / (float)RayCount * Mathf.TwoPi);
			}

		}

		/// <summary>
		/// Calculates the angle-offsets of the player
		/// </summary>
		public Vector3 GetAngles()
		{

			//TODO: This stuff vvv
			/*if (Entity.Get<CharacterComponent>()?.IsGrounded == false)
			{
				return Vector2.Zero;
			}*/

			Vector3 averageDirection = Vector3.Zero;
			Vector3 feetPosition = _player.Position + _feetOffset - Vector3.UnitY * 5f;
			for (int i = 0; i < RayCount; i++)
			{
				Vector3 rayStart = feetPosition + (_offsets[i] * _player.Orientation);
				Vector3 direction = (rayStart - feetPosition);
				direction.Normalize();
				
				RayCastHit hit;
				if (Physics.RayCast(rayStart, /*10f, */direction, out hit, _radius, _player.Layer, false))
				{
					DebugDraw.DrawSphere(hit.Point, 30f, Color.Red);
					averageDirection += _offsets[i] * hit.Distance;
				}
				else
				{
					averageDirection += _offsets[i] * _radius;
				}

			}



			return averageDirection / _radius * 0f;
		}
	}
}
