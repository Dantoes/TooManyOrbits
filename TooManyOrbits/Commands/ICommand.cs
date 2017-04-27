namespace TooManyOrbits.Commands
{
	internal interface ICommand
	{
		void Execute();
		void Undo();
	}
}
