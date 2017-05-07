using System.ComponentModel;
using UnityEngine;

namespace TooManyOrbits
{
	internal class Configuration : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private bool m_hideVesselIcons;
		public bool HideVesselIcons
		{
			get { return m_hideVesselIcons; }
			set { SetValue(ref m_hideVesselIcons, value, nameof(HideVesselIcons)); }
		}

		private bool m_hideVesselOrbits;
		public bool HideVesselOrbits
		{
			get { return m_hideVesselOrbits; }
			set { SetValue(ref m_hideVesselOrbits, value, nameof(HideVesselOrbits)); }
		}

		private bool m_hideCelestialBodyIcons;
		public bool HideCelestialBodyIcons
		{
			get { return m_hideCelestialBodyIcons; }
			set { SetValue(ref m_hideCelestialBodyIcons, value, nameof(HideCelestialBodyIcons)); }
		}

		private bool m_hideCelestialBodyOrbits;
		public bool HideCelestialBodyOrbits
		{
			get { return m_hideCelestialBodyOrbits; }
			set { SetValue(ref m_hideCelestialBodyOrbits, value, nameof(HideCelestialBodyOrbits)); }
		}

		private KeyCode m_toggleKey;
		public KeyCode ToggleKey
		{
			get { return m_toggleKey; }
			set { SetValue(ref m_toggleKey, value, nameof(ToggleKey)); }
		}

		// Window  properties. Do not notify when changed
		public int WindowPositionX { get; set; }
		public int WindowPositionY { get; set; }
		public bool WindowMinimized { get; set; }



		private void SetValue<T>(ref T field, T newValue, string propertyName)
		{
			if (!field?.Equals(newValue) ?? (object) field != (object)newValue)
			{
				field = newValue;
				OnPropertyChanged(propertyName);
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
