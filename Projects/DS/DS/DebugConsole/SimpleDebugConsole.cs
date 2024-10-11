using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DS.DebugConsole
{
	public class SimpleDebugConsole<T> : IDebugConsole<T>
	{
		private readonly Action<T> _log;
		private readonly Dictionary<T, ICommand<T>> _commandDictionary;

		public SimpleDebugConsole(Action<T> log, params ICommand<T>[] commands)
		{
			_log = log;
			Commands = commands.ToHashSet();
			_commandDictionary = new Dictionary<T, ICommand<T>>();
			foreach (var command in Commands)
				AddToCommandDictionary(command);
		}

		public HashSet<ICommand<T>> Commands { get; set; }

		/// <summary>
		/// Adds a command to the list.
		/// </summary>
		/// <param name="command"></param>
		/// <exception cref="ArgumentException">If given a duplicated command or alias</exception>
		public void AddCommand(ICommand<T> command)
		{
			if (!Commands.Add(command))
				throw new DuplicateNameException($"Command {command.Name} has already been added");
			AddToCommandDictionary(command);
		}

		/// <summary>
		/// Adds a command to the list.
		/// </summary>
		/// <param name="command"></param>
		/// <exception cref="DuplicateNameException">If given a duplicate command or alias</exception>
		public bool TryAddCommand(ICommand<T> command)
		{
			if (Commands.Contains(command)
			    || !TryAddToCommandDictionary(command))
				return false;
			Commands.Add(command);
			return true;
		}

		/// <summary>
		/// Searches for a command with name or alias equal to the value given.
		/// </summary>
		/// <param name="name">The name or alias to filter with.</param>
		/// <returns></returns>
		public bool IsValidCommand(T name) => _commandDictionary.ContainsKey(name);

		/// <summary>
		/// Executes the command with the given parameters.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="args"></param>
		public void ExecuteCommand(T name, params T[] args)
		{
			if (!IsValidCommand(name))
				return;
			_commandDictionary[name].Execute(_log, args);
		}

		/// <summary>
		/// Adds a command to the delegates dictionary.
		/// <remarks>Adds an entry to the dictionary for every alias in the command.</remarks>
		/// </summary>
		/// <param name="command"></param>
		/// <exception cref="DuplicateNameException">If given a duplicate command or alias</exception>
		private void AddToCommandDictionary(ICommand<T> command)
		{
			if (!_commandDictionary.ContainsKey(command.Name))
				_commandDictionary.Add(command.Name, command);
			else
				throw new DuplicateNameException($"Command {command.Name} already exists in commands dictionary"); 
			foreach (var alias in command.Aliases)
			{
				if (_commandDictionary.TryGetValue(alias, out var duplicate))
					throw new DuplicateNameException($"A command with alias: {alias} already exists in commands dictionary" +
					                                 $"\n{alias} -> {duplicate.Name}");
				_commandDictionary.Add(alias, command);
			}
		}
		
		/// <summary>
		/// Tries to add a command to the dictionary
		/// </summary>
		/// <param name="command"></param>
		/// <returns>True if command was added succesfully</returns>
		private bool TryAddToCommandDictionary(ICommand<T> command)
		{
			if (!_commandDictionary.ContainsKey(command.Name))
			{
				_commandDictionary.Add(command.Name, command);
				return true;
			}
			foreach (var alias in command.Aliases)
			{
				if (!_commandDictionary.ContainsKey(alias))
				{
					_commandDictionary.Add(alias, command);
					return true;
				}
			}

			return false;
		}
	}
}