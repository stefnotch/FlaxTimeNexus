using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlaxEngine;

namespace FlaxTimeNexus
{
	public class EnableActivator : Script, IActivator
	{
		public Actor ToActivate { get; set; }

		//TODO: Ask about this vvv
		//[ExecuteInEditMode]
		void OnEnable()
		{
			ToActivate.IsActive = true;
		}
	}
}
