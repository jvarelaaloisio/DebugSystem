using System;
using System.Collections.Generic;

namespace DS.DebugConsole
{
	public class SimpleDebugConsole<T> : IDebugConsole<T>
	{
		private readonly Action<T> _giveFeedback;
		private readonly Dictionary<T, Action<T[], Action<T>>> _commandDictionary;

		public SimpleDebugConsole(Action<T> giveFeedback, List<ICommand<T>> commands)
		{
			_giveFeedback = giveFeedback;
			Commands = commands;
			_commandDictionary = new Dictionary<T, Action<T[], Action<T>>>();
			foreach (var command in Commands)
			{
				AddToCommandDictionary(command);
			}
		}

		public List<ICommand<T>> Commands { get; set; }

		public void AddCommand(ICommand<T> command)
		{
			if (Commands.Contains(command))
				return;
			Commands.Add(command);
			AddToCommandDictionary(command);
		}

		public bool IsValidCommand(T name) => (_commandDictionary.ContainsKey(name));

		public void ExecuteCommand(T name, T[] args)
		{
			if (!IsValidCommand(name))
				return;
			_commandDictionary[name].Invoke(args, _giveFeedback);
		}

		private void AddToCommandDictionary(ICommand<T> command)
		{
			if (!_commandDictionary.ContainsKey(command.Name))
				_commandDictionary.Add(command.Name, command.Execute);
			foreach (var alias in command.Aliases)
			{
				if (_commandDictionary.ContainsKey(alias))
					return;
				_commandDictionary.Add(alias, command.Execute);
			}
		}
	}
}