using System.Diagnostics;

namespace TooManyOrbits
{
	internal static class Logger
	{
		private static readonly string LogPrefix;

		static Logger()
		{
			LogPrefix = $"[{TooManyOrbitsModule.ModName}] ";
		}

		private static string FormatMessage(string message)
		{
			return LogPrefix + message;
		}

		public static void Info(string message)
		{
			UnityEngine.Debug.Log(FormatMessage(message));
		}

		public static void Warn(string message)
		{
			UnityEngine.Debug.LogWarning(FormatMessage(message));
		}

		public static void Error(string message)
		{
			UnityEngine.Debug.LogError(FormatMessage(message));
		}

		[Conditional("DEBUG")]
		public static void Debug(string message)
		{
			UnityEngine.Debug.Log(FormatMessage(message));
		}
	}
}
