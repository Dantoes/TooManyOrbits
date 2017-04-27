using System;

namespace TooManyOrbits
{
	internal interface IVisibilityController : IDisposable
	{
		bool IsVisible { get; }
		void Show();
		void Hide();
		void Toggle();
	}
}
