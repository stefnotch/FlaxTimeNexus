using FlaxEngine;

namespace FlaxTimeNexus
{
	public interface ILookatTrigger
	{
		void OnLookatEnter(RayCastHit hitResult);
		void OnLookatStay(RayCastHit hitResult);
		void OnLookatExit(RayCastHit hitResult);
	}
}