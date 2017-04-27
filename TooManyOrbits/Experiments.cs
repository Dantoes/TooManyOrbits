using System.Reflection;
using UnityEngine;

namespace TooManyOrbits
{
	// just attempts to get things to work in this class
	internal static class Experiments
	{
		public static void LogTargetInfo(ITargetable target)
		{
			var method = typeof(OrbitRenderer).GetMethod("GetOrbitColour", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			Color orbitColor2 = (Color)method.Invoke(target.GetOrbitDriver().Renderer, new object[0]);

			Logger.Debug("Target name: " + target.GetName());
			Logger.Debug("Target orbit color 1: " + target.GetOrbitDriver().Renderer.orbitColor);
			Logger.Debug("Target orbit color 2: " + orbitColor2);
			Logger.Debug("Target in vessel list: " + FlightGlobals.Vessels.Contains(target.GetVessel()));
			Logger.Debug("Target same orbit renderer: " + (target.GetOrbitDriver().Renderer == target.GetVessel().orbitRenderer));
			Logger.Debug("Target orbit renderer mode: " + target.GetOrbitDriver().Renderer.drawMode);
			Logger.Debug("Target isActive: " + target.GetOrbitDriver().Renderer.isActive);
			Logger.Debug("Target isFocused: " + target.GetOrbitDriver().Renderer.isFocused);
			Logger.Debug("Target orbit driver draw: " + target.GetOrbitDriver().drawOrbit);
			Logger.Debug("Target orbit driver update mode: " + target.GetOrbitDriver().updateMode);
			Logger.Debug("Target orbit driver last mode: " + target.GetOrbitDriver().lastMode);
			Logger.Debug("Target orbit driver color: " + target.GetOrbitDriver().orbitColor);
			Logger.Debug("Target has conic renderer: " + (target.GetVessel().patchedConicRenderer != null));

		}

		#region Test 1: attempt to hide vessel target
		public static void HideTargetVessel_v1()
		{
			var target = FlightGlobals.ActiveVessel.targetObject;
			if (target != null)
			{
				var conicRenderer = target.GetVessel().patchedConicRenderer;
				if (conicRenderer != null)
				{
					conicRenderer.patchRenders.ForEach(r => r.Terminate());
					conicRenderer.flightPlanRenders.ForEach(r => r.Terminate());
					conicRenderer.enabled = false;
					//UnityEngine.Object.Destroy(conicRenderer);
				}

				var orbitTargeter = target.GetVessel().orbitTargeter;
				if (orbitTargeter != null)
				{
					UnityEngine.Object.Destroy(orbitTargeter);
				}

				UnityEngine.Object.Destroy(FlightGlobals.ActiveVessel.orbitTargeter);
			}
		}

		public static void ShowTargetVessel_v1()
		{
			var target = FlightGlobals.ActiveVessel.targetObject;
			if (target != null)
			{
				var conicRenderer = target.GetVessel().patchedConicRenderer;
				if (conicRenderer != null)
				{
					conicRenderer.enabled = true;
				}

				target.GetVessel().orbitTargeter = target.GetVessel().gameObject.AddComponent<OrbitTargeter>();
				FlightGlobals.ActiveVessel.orbitTargeter = FlightGlobals.ActiveVessel.gameObject.AddComponent<OrbitTargeter>();
			}
		}
		#endregion

		#region Test 2: attempt to hide vessel target
		public static void HideTargetVessel_v2()
		{
			var target = FlightGlobals.ActiveVessel.targetObject;
			if (target != null)
			{
				target.GetVessel().DetachPatchedConicsSolver();
			}
		}

		public static void ShowTargetVessel_v2()
		{
			var target = FlightGlobals.ActiveVessel.targetObject;
			if (target != null)
			{
				target.GetVessel().AttachPatchedConicsSolver();
			}
		}
		#endregion
	}
}
