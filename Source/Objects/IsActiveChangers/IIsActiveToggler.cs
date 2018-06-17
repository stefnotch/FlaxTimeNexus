using FlaxEngine;

namespace FlaxTimeNexus
{
	public interface IIsActiveToggler
	{
		Actor ToActivate { get; set; }
		Actor ToDeactivate { get; set; }
	}
}
