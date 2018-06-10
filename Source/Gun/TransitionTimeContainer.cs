using FlaxEngine;

namespace FlaxTimeNexus
{
	//TODO: Rename this to something better
	internal class TransitionTimeContainer
	{
		public readonly TimeContainer TimeContainer;
		public float TransitionValue = 0f;
		public RayCastHit Hit;
		public SDateTime ToAdd = SDateTime.Zero;

		public TransitionTimeContainer(TimeContainer timeContainer)
		{
			this.TimeContainer = timeContainer;
		}
	}
}