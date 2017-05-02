using UnityEngine;

namespace TooManyOrbits
{
	internal class ResourceProvider
	{
		private readonly string m_resourcePath;

		public Texture ToolbarIcon => LoadTextureResource("ToolbarIcon");

		public ResourceProvider(string resourcePath)
		{
			m_resourcePath = resourcePath;
			if (!m_resourcePath.EndsWith("/"))
			{
				m_resourcePath += '/';
			}
		}

		private Texture LoadTextureResource(string resourceName)
		{
			string path = BuildPath(resourceName);
			Logger.Debug("Loading texture: " + path);

			var texture = GameDatabase.Instance.GetTexture(path, false);
			if (texture == null)
			{
				Logger.Error("Failed to load texture " + resourceName);
			}
			return texture;
		}

		private string BuildPath(string resourceName)
		{
			return m_resourcePath + resourceName;
		}
	}
}
