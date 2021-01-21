using System.Collections.Generic;

namespace DS.DebugConsole
{
	public interface IDebugConsole<T>
	{
		List<ICommand<T>> Commands { get; set; }
		void AddCommand(ICommand<T> command);
		bool IsValidCommand(T name);
		void ExecuteCommand(T name, T[] args);
	}
}