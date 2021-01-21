using System.Collections.Generic;
using DS.DebugConsole;
using Moq;

namespace DS.UnitTests
{
	public static class A
	{
		public static Mock<ICommand<string>> MockCommand(string commandMockName)
		{
			var mock = new Mock<ICommand<string>>();
			mock.SetupGet(c => c.Name)
				.Returns(commandMockName);
			mock.SetupGet(c => c.Aliases)
				.Returns(new List<string>());
			return mock;
		}

		public static Mock<ICommand<string>> WithAlias(this Mock<ICommand<string>> command, string alias)
		{
			command.SetupGet(c => c.Aliases)
				.Returns(new List<string> {alias});
			return command;
		}
	}
}