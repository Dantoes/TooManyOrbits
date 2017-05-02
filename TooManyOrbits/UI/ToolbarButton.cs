using System;
using KSP.UI.Screens;
using UnityEngine;

namespace TooManyOrbits.UI
{
	internal class ToolbarButton : IDisposable
	{
		public event Callback OnEnable, OnDisable;

		private readonly Texture m_texture;
		private ApplicationLauncherButton m_button;

		public ToolbarButton(ResourceProvider resources)
		{
			m_texture = resources.ToolbarIcon;
		}

		public void Dispose()
		{
			Hide();
		}

		public void Show()
		{
			if (m_button == null)
			{
				m_button = BuildButton();

			}
		}

		public void Hide()
		{
			if (m_button != null)
			{
				DestroyButton(m_button);
				m_button = null;
			}
		}

		private ApplicationLauncherButton BuildButton()
		{
			return ApplicationLauncher.Instance.AddModApplication(
				onTrue: () => OnEnable?.Invoke(),
				onFalse: () => OnDisable?.Invoke(),
				onHover: null,
				onHoverOut: null,
				onEnable: null,
				onDisable: null,
				visibleInScenes: ApplicationLauncher.AppScenes.ALWAYS, /* MAPVIEW does not work in 1.2.2 */
				texture: m_texture
				);
		}

		private void DestroyButton(ApplicationLauncherButton button)
		{
			ApplicationLauncher.Instance.RemoveModApplication(button);
		}
	}
}
