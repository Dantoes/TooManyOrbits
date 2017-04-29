namespace TooManyOrbits.Filter
{
	internal class VesselFilter : IFilter<Vessel>
	{
		public bool Accept(Vessel obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj == FlightGlobals.ActiveVessel)
			{
				return false;
			}
			if (obj == FlightGlobals.ActiveVessel.targetObject?.GetVessel())
			{
				return false;
			}
			return true;
		}
	}
}
