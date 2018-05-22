using FlaxEngine;

namespace BasicTemplate
{
	public interface ILookatTrigger
	{
		void OnLookatEnter(RayCastHit hitResult);
		void OnLookatStay(RayCastHit hitResult);
		void OnLookatExit(RayCastHit hitResult);
	}
}