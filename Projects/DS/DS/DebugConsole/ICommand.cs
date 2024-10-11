using System;
using System.Collections.Generic;

namespace DS.DebugConsole
{
	public interface ICommand<T>
	{
		T Name { get; }
		IEnumerable<T> Aliases { get; }
		T Description { get; }
		void Execute(Action<T> giveFeedBack, params T[] args);
	}
}