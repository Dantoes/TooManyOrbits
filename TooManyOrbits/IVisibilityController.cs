using System;

namespace TooManyOrbits
{
	internal interface IVisibilityController : IDisposable
	{
		event Callback<bool> OnVisibilityChanged;

		bool IsVisible { get; }
		void Show();
		void Hide();
		void Toggle();
	}
}
