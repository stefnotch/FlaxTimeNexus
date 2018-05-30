using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaxTimeNexus
{
	public interface IIsActiveChanger
	{
		Actor ToActivate { get; set; }
	}
}
