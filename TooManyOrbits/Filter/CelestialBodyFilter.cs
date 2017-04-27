namespace TooManyOrbits.Filter
{
	internal class CelestialBodyFilter : IFilter<CelestialBody>
	{
		public bool Accect(CelestialBody obj)
		{
			var targetedCelestialBody = FlightGlobals.ActiveVessel.targetObject?.GetOrbitDriver().celestialBody;
			if (obj == targetedCelestialBody)
			{
				return false;
			}

			return obj.GetOrbitDriver()?.Renderer != null;
		}
	}
}
