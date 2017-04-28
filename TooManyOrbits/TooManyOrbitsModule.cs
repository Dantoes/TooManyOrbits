using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using TooManyOrbits.UI;
using UnityEngine;

namespace TooManyOrbits
{
	[KSPAddon(KSPAddon.Startup.Flight, false)]
    public class TooManyOrbitsModule : MonoBehaviour
	{
		public const string ModName = "TooManyOrbits";
		private const KeyCode ToggleButton = KeyCode.F8;

		private string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
		private string ConfigurationFile => $"GameData/{ModName}/{ModName}.cfg";

		private ToolbarButton m_toolbarButton;
		private ConfigurationWindow m_window;
		private Configuration m_configuration;
		private IVisibilityController m_visibilityController;
		private bool m_lastVisibilityState = true;

		private void Start()
		{
			Logger.Info($"Starting {ModName} v{Version}...");

			Logger.Debug("Loading configuration");
			m_configuration = ConfigurationParser.LoadFromFile(ConfigurationFile);

			Logger.Debug("Setting up OrbitVisibilityController");
			m_visibilityController = new OrbitVisibilityController(m_configuration);
			m_visibilityController.OnVisibilityChanged += OnOrbitVisibilityChanged;

			// setup window
			Logger.Debug("Creating window");
			m_window = new ConfigurationWindow($"{ModName} Configuration", m_configuration, m_visibilityController);

			// setup toolbar button
			Logger.Debug("Creating toolbar button");
			m_toolbarButton = new ToolbarButton($"{ModName}/ToolbarIcon");
			m_toolbarButton.OnEnable += m_window.Show;
			m_toolbarButton.OnDisable += m_window.Hide;

			// get notifcations when player changes to map view
			MapView.OnEnterMapView += OnEnterMapView;
			MapView.OnExitMapView += OnExitMapView;
		}

		[SuppressMessage("ReSharper", "DelegateSubtraction")]
		private void OnDestroy()
		{
			Logger.Info("Shutting down");
			MapView.OnEnterMapView -= OnEnterMapView;
			MapView.OnExitMapView -= OnExitMapView;

			Logger.Debug("Disposing OrbitVisibilityController");
			m_visibilityController.OnVisibilityChanged -= OnOrbitVisibilityChanged;
			m_visibilityController.Dispose();

			Logger.Debug("Disposing ToolbarButton");
			m_toolbarButton.Dispose();

			Logger.Debug("Writing configuration file");
			ConfigurationParser.SaveToFile(ConfigurationFile, m_configuration);
		}

		private void Update()
		{
			if (Input.GetKeyDown(ToggleButton))
			{
				m_visibilityController.Toggle();
			}
		}

		private void OnEnterMapView()
		{
			Logger.Debug("Enabling...");
			enabled = true;

			if (!m_lastVisibilityState)
			{
				m_visibilityController.Hide();
			}
			m_toolbarButton.Show();
		}

		private void OnExitMapView()
		{
			Logger.Debug("Disabling...");
			enabled = false;

			m_lastVisibilityState = m_visibilityController.IsVisible;
			m_visibilityController.Show();
			m_toolbarButton.Hide();
		}

		private void OnOrbitVisibilityChanged(bool orbitsVisible)
		{
			const float duration = 1.5f;
			if (enabled)
			{
				var message = orbitsVisible ? "Orbits shown" : "Orbits hidden";
				ScreenMessages.PostScreenMessage(message, duration);
			}
		}

		private void OnGUI()
		{
			var originalSkin = GUI.skin;
			GUI.skin = HighLogic.Skin;

			m_window.Draw();

			GUI.skin = originalSkin;
		}
	}
}
