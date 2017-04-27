using System;
using UnityEngine;

namespace TooManyOrbits.UI
{
	internal class ConfigurationWindow
	{
		const int Width = 300;
		const int Height = 200;

		private readonly int m_windowId = Guid.NewGuid().GetHashCode();
		private readonly string m_title;
		private readonly Configuration m_configuration;
		private readonly IVisibilityController m_visibilityController;

		private bool m_visible = false;
		private Rect m_position;


		public ConfigurationWindow(string title, Configuration configuration, IVisibilityController visibilityController)
		{
			m_title = title;
			m_configuration = configuration;
			m_visibilityController = visibilityController;
			m_position = CalculateCenterPosition();
		}

		public void Show()
		{
			m_visible = true;
		}

		public void Hide()
		{
			m_visible = false;
		}

		public void Draw()
		{
			if (m_visible)
			{
				DrawWindow();
			}
		}

		private void DrawWindow()
		{
			m_position = GUILayout.Window(m_windowId, m_position, DrawWindowContent, m_title);
		}

		private void DrawWindowContent(int windowId)
		{
			GUILayout.BeginVertical();
			DrawCloseButton();

			bool enabled = !m_visibilityController.IsVisible;
			bool shouldEnable = GUILayout.Toggle(enabled, "Enabled (press F8 to toggle)");
			if (enabled != shouldEnable)
			{
				m_visibilityController.Toggle();
			}

			GUILayout.Space(20);
			m_configuration.HideVesselIcons = GUILayout.Toggle(m_configuration.HideVesselIcons, "Hide vessel icons");
			m_configuration.HideVesselOrbits = GUILayout.Toggle(m_configuration.HideVesselOrbits, "Hide vessel orbits");
			m_configuration.HideCelestialBodyIcons = GUILayout.Toggle(m_configuration.HideCelestialBodyIcons, "Hide celestial body icons");
			m_configuration.HideCelestialBodyOrbits = GUILayout.Toggle(m_configuration.HideCelestialBodyOrbits, "Hide celestial body orbits");
			GUILayout.EndHorizontal();

			GUI.DragWindow();
		}

		private void DrawCloseButton()
		{
			const int offset = 5;
			const int size = 20;

			var position = new Rect(m_position.width - size - offset, offset, size, size);
			if (GUI.Button(position, "X"))
			{
				Hide();
			}
		}

		private Rect CalculateCenterPosition()
		{
			return new Rect((Screen.width - Width) / 2F, (Screen.height - Height) / 2F, Width, Height);
		}
	}
}
