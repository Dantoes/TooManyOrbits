using System.ComponentModel;
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
		public const string ResourcePath = ModName + "/";

		private string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
		private string ConfigurationFile => $"GameData/{ModName}/{ModName}.cfg";

		private KeyCode m_toggleButton;
		private ToolbarButton m_toolbarButton;
		private ConfigurationWindow m_window;
		private Configuration m_configuration;
		private IVisibilityController m_visibilityController;
		private bool m_lastVisibilityState = true;
		private bool m_skipUpdate = false;

		private void Start()
		{
			Logger.Info($"Starting {ModName} v{Version}...");

			var resourceProvider = new ResourceProvider(ModName);

			Logger.Debug("Loading configuration");
			m_configuration = ConfigurationParser.LoadFromFile(ConfigurationFile);
			m_configuration.PropertyChanged += OnConfigurationChanged;
			m_toggleButton = m_configuration.ToggleKey;

			Logger.Debug("Setting up OrbitVisibilityController");
			m_visibilityController = new OrbitVisibilityController(m_configuration);
			m_visibilityController.OnVisibilityChanged += OnOrbitVisibilityChanged;

			// setup window
			Logger.Debug("Creating window");
			m_window = new ConfigurationWindow($"{ModName} Configuration", m_configuration, m_visibilityController, resourceProvider);

			// setup toolbar button
			Logger.Debug("Creating toolbar button");
			m_toolbarButton = new ToolbarButton(resourceProvider);
			m_toolbarButton.OnEnable += m_window.Show;
			m_toolbarButton.OnDisable += m_window.Hide;

			// get notifcations when player changes to map view
			MapView.OnEnterMapView += OnEnterMapView;
			MapView.OnExitMapView += OnExitMapView;

			// disable script until woken up by entering map view
			enabled = false;
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
			if (m_skipUpdate)
			{
				m_skipUpdate = false;
				return;
			}

			if (Input.GetKeyDown(m_toggleButton))
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

		private void OnConfigurationChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args?.PropertyName == nameof(Configuration.ToggleKey))
			{
				m_toggleButton = m_configuration.ToggleKey;
				m_skipUpdate = true;
				Logger.Info($"Changed toggle key to '{m_configuration.ToggleKey}'");
			}
		}
	}
}
