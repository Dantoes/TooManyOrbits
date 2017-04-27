using System.Collections.Generic;

namespace TooManyOrbits.Commands
{
	internal class CommandSet : ICommand
	{
		private readonly IList<ICommand> m_commands = new List<ICommand>(16);
		private readonly Stack<ICommand> m_executedCommands = new Stack<ICommand>(16);

		public void Add(ICommand command)
		{
			m_commands.Add(command);
		}

		public void Execute()
		{
			if (m_executedCommands.Count > 0)
			{
				Logger.Error("CommandSet is in an invalid state");
			}

			for (int i = 0; i < m_commands.Count; i++)
			{
				m_commands[i].Execute();
				m_executedCommands.Push(m_commands[i]);
			}
		}

		public void Undo()
		{
			while (m_executedCommands.Count > 0)
			{
				m_executedCommands.Pop().Undo();
			}
		}
	}
}
