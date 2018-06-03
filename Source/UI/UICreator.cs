using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;

namespace FlaxTimeNexus
{
	public abstract class UICreator : Script
	{
		public abstract ContainerControl CreateUI(Vector2 size);
	}
}
