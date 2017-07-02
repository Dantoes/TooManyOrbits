namespace TooManyOrbits.Filter
{
	internal class CelestialBodyFilter : IFilter<CelestialBody>
	{
		public bool Accept(CelestialBody obj)
		{
			var targetedCelestialBody = FlightGlobals.ActiveVessel?.targetObject?.GetOrbitDriver()?.celestialBody;
			if (obj == targetedCelestialBody)
			{
				return false;
			}

			return obj.GetOrbitDriver()?.Renderer != null;
		}
	}
}
