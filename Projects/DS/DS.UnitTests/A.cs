using System.Collections.Generic;
using DS.DebugConsole;
using Moq;

namespace DS.UnitTests
{
	/// <summary>
	/// Builder class for mocks
	/// <remarks>It's called "A" so code is read like english speech</remarks>
	/// </summary>
	public static class A
	{
		/// <summary>
		/// Create a mock for a command that simply contains the given name.
		/// </summary>
		/// <param name="commandMockName"></param>
		/// <returns></returns>
		public static Mock<ICommand<string>> MockCommand(string commandMockName)
		{
			var mock = new Mock<ICommand<string>>();
			mock.SetupGet(c => c.Name)
				.Returns(commandMockName);
			mock.SetupGet(c => c.Aliases)
				.Returns(new List<string>());
			return mock;
		}

		/// <summary>
		/// Sets the collection of aliases for the specific command 
		/// </summary>
		/// <param name="command"></param>
		/// <param name="aliases"></param>
		/// <returns></returns>
		public static Mock<ICommand<string>> WithAlias(this Mock<ICommand<string>> command, params string[] aliases)
		{
			command.SetupGet(c => c.Aliases)
				.Returns(aliases);
			return command;
		}
	}
}