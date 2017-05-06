using System;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyOrbits.UI
{
	internal class ConfigurationWindow : IDisposable
	{
		const int Width = 300;
		const int Height = 250;

		private static readonly IList<KeyCode> AllowedKeyCodes = GetAllowedKeyCodes(); 

		private readonly int m_windowId = Guid.NewGuid().GetHashCode();
		private readonly string m_title;
		private readonly Configuration m_configuration;
		private readonly IVisibilityController m_visibilityController;
		private readonly Texture m_pencilTexture;

		private GUIStyle m_textfieldStyle;
		private bool m_visible = false;
		private bool m_setKeyMode = false;
		private Rect m_position;


		public ConfigurationWindow(string title, Configuration configuration, IVisibilityController visibilityController, ResourceProvider resources)
		{
			m_title = title;
			m_configuration = configuration;
			m_visibilityController = visibilityController;
			m_pencilTexture = resources.PencilIcon;
			RestoreWindowPosition();
		}

		public void Dispose()
		{
			SaveWindowPosition();
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
			bool shouldEnable = GUILayout.Toggle(enabled, "Enabled");
			if (enabled != shouldEnable)
			{
				m_visibilityController.Toggle();
			}

			GUILayout.Space(20);
			m_configuration.HideVesselIcons = GUILayout.Toggle(m_configuration.HideVesselIcons, "Hide vessel icons");
			m_configuration.HideVesselOrbits = GUILayout.Toggle(m_configuration.HideVesselOrbits, "Hide vessel orbits");
			m_configuration.HideCelestialBodyIcons = GUILayout.Toggle(m_configuration.HideCelestialBodyIcons, "Hide celestial body icons");
			m_configuration.HideCelestialBodyOrbits = GUILayout.Toggle(m_configuration.HideCelestialBodyOrbits, "Hide celestial body orbits");
			GUILayout.Space(20);
			DrawKeyBinding();
			GUILayout.EndVertical();

			GUI.DragWindow();
		}

		private void DrawKeyBinding()
		{
			const int buttonWidth = 22;

			if (m_textfieldStyle == null) // must reside in a function called by OnGui
			{
				m_textfieldStyle = GUI.skin.GetStyle("TextField");
			}

			GUILayout.BeginHorizontal();
			GUILayout.Label("Hotkey: ");
			GUILayout.Space(10);

			if (m_setKeyMode)
			{
				GUILayout.Label("<Press a key>", m_textfieldStyle);
				KeyCode? newKeyCode = GetNextPressedKey();

				if (newKeyCode.HasValue)
				{
					m_configuration.ToggleKey = newKeyCode.Value;
					m_setKeyMode = false;
				}

				if (GUILayout.Button("X", GUILayout.Width(buttonWidth), GUILayout.Height(buttonWidth)))
				{
					m_setKeyMode = false;
				}
			}
			else
			{
				GUILayout.Label(m_configuration.ToggleKey.ToString(), m_textfieldStyle);
				m_setKeyMode = GUILayout.Button(m_pencilTexture, GUILayout.Width(buttonWidth), GUILayout.Height(buttonWidth));
			}
			GUILayout.EndHorizontal();
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

		private KeyCode? GetNextPressedKey()
		{
			for (int i = 0; i < AllowedKeyCodes.Count; i++)
			{
				if (Input.GetKeyDown(AllowedKeyCodes[i]))
				{
					return AllowedKeyCodes[i];
				}
			}
			return null;
		}

		private void SaveWindowPosition()
		{
			m_configuration.WindowPositionX = Mathf.FloorToInt(m_position.xMin);
			m_configuration.WindowPositionY = Mathf.FloorToInt(m_position.yMin);
		}

		private void RestoreWindowPosition()
		{
			int x = m_configuration.WindowPositionX;
			int y = m_configuration.WindowPositionY;

			if (x < 0 || y < 0) // position invalid or not set
			{
				m_position = CalculateCenterPosition();
			}
			else
			{
				m_position = new Rect(x, y, Width, Height);

				if (m_position.xMax > Screen.width)
				{
					m_position.x = Screen.width - Width;
				}
				if (m_position.yMax > Screen.height)
				{
					m_position.y = Screen.height - Height;
				}
			}
		}

		private static IList<KeyCode> GetAllowedKeyCodes()
		{
			var keys = new List<KeyCode>(128);

			foreach (var value in Enum.GetValues(typeof (KeyCode)))
			{
				var keyCode = (KeyCode)value;
				if (!keyCode.ToString().StartsWith("Mouse")
				    && keyCode != KeyCode.Escape)
				{
					keys.Add(keyCode);
				}
			}

			return keys;
		}

	}
}
