namespace TooManyOrbits.Commands
{
	internal class HideOrbitCommand : ICommand
	{
		private readonly OrbitRendererBase m_renderer;
		private readonly bool m_hideIcon;
		private readonly bool m_hideOrbit;

		private readonly OrbitRenderer.DrawIcons m_originalIconMode;
		private readonly OrbitRenderer.DrawMode m_originalOrbitMode;

		public HideOrbitCommand(OrbitRendererBase renderer, bool hideIcon, bool hideOrbit)
		{
			m_renderer = renderer;
			m_hideIcon = hideIcon;
			m_hideOrbit = hideOrbit;
			m_originalIconMode = m_renderer.drawIcons;
			m_originalOrbitMode = m_renderer.drawMode;
		}

		public void Execute()
		{
			if (m_hideIcon)
			{
				m_renderer.drawIcons = OrbitRenderer.DrawIcons.NONE;
			}
			if (m_hideOrbit)
			{
				m_renderer.drawMode = OrbitRenderer.DrawMode.OFF;
			}
		}

		public void Undo()
		{
			if (m_hideIcon)
			{
				m_renderer.drawIcons = m_originalIconMode;
			}
			if (m_hideOrbit)
			{
				m_renderer.drawMode = m_originalOrbitMode;
			}
		}
	}
}
