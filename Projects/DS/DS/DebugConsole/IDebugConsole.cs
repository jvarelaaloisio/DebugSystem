using System.Collections.Generic;
using System.Data;

namespace DS.DebugConsole
{
	public interface IDebugConsole<T>
	{
		HashSet<ICommand<T>> Commands { get; set; }
		void AddCommand(ICommand<T> command);
		bool IsValidCommand(T name);
		void ExecuteCommand(T name, params T[] args);

		/// <summary>
		/// Adds a command to the list.
		/// </summary>
		/// <param name="command"></param>
		/// <exception cref="DuplicateNameException">If given a duplicate command or alias</exception>
		bool TryAddCommand(ICommand<T> command);
	}
}