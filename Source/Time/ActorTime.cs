﻿using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	public class ActorTime : Script
	{
		/// <summary>
		/// The time of the actor, can be modified in the editor
		/// </summary>
		[Serialize]
		//[EditorDisplay("Time", "__inline__")]
		public SDateTime EditorTime { get; set; } = SDateTime.Zero;

		/// <summary>
		/// The time of the actor
		/// </summary>
		[HideInEditor]
		[NoSerialize]
		public SDateTime Time
		{
			get
			{
				return EditorTime;
			}
		}
	}
}
