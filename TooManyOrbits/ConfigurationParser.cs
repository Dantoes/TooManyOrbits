using System;
using System.Collections.Generic;

namespace TooManyOrbits
{

	internal static class ConfigurationParser
	{
		struct Key
		{
			public string Name;
			public string DefaultValue;
			public Func<Configuration, string> PropertyGetter;
			public Action<Configuration, string> PropertySetter;
		}

		private static readonly IList<Key> Keys = new List<Key>();

		static ConfigurationParser()
		{
			RegisterConfigKey(nameof(Configuration.HideVesselIcons), true, Convert.ToBoolean);
			RegisterConfigKey(nameof(Configuration.HideVesselOrbits), true, Convert.ToBoolean);
			RegisterConfigKey(nameof(Configuration.HideCelestialBodyIcons), true, Convert.ToBoolean);
			RegisterConfigKey(nameof(Configuration.HideCelestialBodyOrbits), false, Convert.ToBoolean);
		}

		private static void RegisterConfigKey<T>(string name, T defaultValue, Func<string, T> converter)
		{
			var property = typeof(Configuration).GetProperty(name);
			var key = new Key
			{
				Name = name,
				DefaultValue = defaultValue.ToString(),
				PropertyGetter = configuration => property.GetValue(configuration, null)?.ToString(),
				PropertySetter = (configuration, s) => property.SetValue(configuration, converter(s), null),
			};
			Keys.Add(key);
		}

		public static Configuration LoadFromFile(string path)
		{
			ConfigNode configNode = ConfigNode.Load(path);
			Configuration configuration = new Configuration();

			foreach (var key in Keys)
			{
				string value = configNode?.GetValue(key.Name) ?? key.DefaultValue;
				key.PropertySetter(configuration, value);
			}

			return configuration;
		}

		public static void SaveToFile(string path, Configuration configuration)
		{
			ConfigNode configNode = new ConfigNode("Configuration");
			foreach (var key in Keys)
			{
				string value = key.PropertyGetter(configuration) ?? key.DefaultValue;
				configNode.SetValue(key.Name, value, true);
			}
			
			configNode.Save(path);
		}
	}
}