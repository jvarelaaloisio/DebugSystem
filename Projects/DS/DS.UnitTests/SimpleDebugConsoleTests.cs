using System.Collections.Generic;
using DS.DebugConsole;
using Moq;
using NUnit.Framework;

namespace DS.UnitTests
{
	public class SimpleDebugConsoleTests
	{
		[Test]
		public void Commands_NoCommandsAdded_ReturnsEmptyList()
		{
			IDebugConsole<string> debugConsole = new SimpleDebugConsole<string>(null, new List<ICommand<string>>());
			Assert.IsNotNull(debugConsole.Commands);
			Assert.IsEmpty(debugConsole.Commands);
		}

		[Test]
		public void Commands_OneCommandAdded_ReturnsListWithCommand()
		{
			const string mockName = "name";
			var mockCommand = A.MockCommand(mockName);
			IDebugConsole<string> debugConsole =
				new SimpleDebugConsole<string>(null, new List<ICommand<string>> {mockCommand.Object});
			Assert.IsNotNull(debugConsole.Commands);
			Assert.IsNotEmpty(debugConsole.Commands);
			Assert.AreEqual(1, debugConsole.Commands.Count);
			Assert.AreEqual(mockName, debugConsole.Commands[0].Name);
		}

		[Test]
		public void Commands_CopyFromOuterList_ReturnsSameList()
		{
			const string commandName = "name";
			var mockCommand = A.MockCommand(commandName);
			IDebugConsole<string> debugConsole = new SimpleDebugConsole<string>(null, new List<ICommand<string>>());
			var commands = new List<ICommand<string>> {mockCommand.Object};
			debugConsole.Commands = commands;
			Assert.IsNotEmpty(debugConsole.Commands);
			Assert.AreEqual(1, debugConsole.Commands.Count);
			Assert.AreEqual(commandName, debugConsole.Commands[0].Name);
		}

		[Test]
		public void AddCommand_AddOneCommand_ListContainsCommand()
		{
			const string commandName = "name";
			var mockCommand = A.MockCommand(commandName);
			IDebugConsole<string> debugConsole = new SimpleDebugConsole<string>(null, new List<ICommand<string>>());
			debugConsole.AddCommand(mockCommand.Object);
			Assert.IsNotEmpty(debugConsole.Commands);
			Assert.AreEqual(1, debugConsole.Commands.Count);
			Assert.AreEqual(commandName, debugConsole.Commands[0].Name);
		}

		[Test]
		public void AddCommand_AddOneCommand_IsValidCommand()
		{
			const string commandName = "name";
			var mockCommand = A.MockCommand(commandName);
			IDebugConsole<string> debugConsole = new SimpleDebugConsole<string>(null, new List<ICommand<string>>());
			debugConsole.AddCommand(mockCommand.Object);
			Assert.IsTrue(debugConsole.IsValidCommand(commandName));
		}

		[Test]
		public void AddCommand_AddOneCommandWithAlias_AliasIsValidCommand()
		{
			const string commandName = "name";
			const string alias = "alias";
			var mockCommand = A.MockCommand(commandName).WithAlias(alias);
			IDebugConsole<string> debugConsole = new SimpleDebugConsole<string>(null, new List<ICommand<string>>());
			debugConsole.AddCommand(mockCommand.Object);
			Assert.IsTrue(debugConsole.IsValidCommand(alias));
		}

		[Test]
		public void AddCommand_AddCommandThatIsAlreadyInList_ListContainsCommandOnce()
		{
			const string commandName = "name";
			var mockCommand = A.MockCommand(commandName);
			IDebugConsole<string> debugConsole =
				new SimpleDebugConsole<string>(null, new List<ICommand<string>> {mockCommand.Object});
			debugConsole.AddCommand(mockCommand.Object);
			Assert.IsNotEmpty(debugConsole.Commands);
			Assert.AreEqual(1, debugConsole.Commands.Count);
			Assert.AreEqual(commandName, debugConsole.Commands[0].Name);
		}

		[Test]
		public void IsValidCommand_CommandDoesNotExist_ReturnsFalse()
		{
			const string mockName = "name";
			const string otherName = "otherName";
			var mock = A.MockCommand(mockName);

			IDebugConsole<string> debugConsole = new SimpleDebugConsole<string>(null, new List<ICommand<string>>
				{mock.Object});

			Assert.IsFalse(debugConsole.IsValidCommand(otherName));
		}

		[Test]
		public void IsValidCommand_CommandExists_ReturnsTrue()
		{
			const string mockName = "name";
			var mock = A.MockCommand(mockName);
			IDebugConsole<string> debugConsole = new SimpleDebugConsole<string>(null, new List<ICommand<string>>
				{mock.Object});
			Assert.IsTrue(debugConsole.IsValidCommand(mockName));
		}

		[Test]
		public void ExecuteCommand_CommandDoesNotExist_DoesNotExecute()
		{
			const string mockName = "name";
			const string otherName = "otherName";
			var mockCommand = A.MockCommand(mockName);
			IDebugConsole<string> debugConsole =
				new SimpleDebugConsole<string>(null, new List<ICommand<string>> {mockCommand.Object});
			debugConsole.ExecuteCommand(otherName, null);
			mockCommand.Verify(c => c.Execute(null, null), Times.Never);
		}

		[Test]
		public void ExecuteCommand_CommandDoesExists_Executes()
		{
			const string mockName = "name";
			var mockCommand = A.MockCommand(mockName);
			IDebugConsole<string> debugConsole =
				new SimpleDebugConsole<string>(null, new List<ICommand<string>> {mockCommand.Object});
			debugConsole.ExecuteCommand(mockName, null);
			mockCommand.Verify(c => c.Execute(null, null), Times.Once);
		}

		[Test]
		public void ExecuteCommand_ParameterIsCommandAlias_Executes()
		{
			const string mockName = "name";
			const string alias = "alias";
			var mockCommand = A.MockCommand(mockName).WithAlias(alias);
			IDebugConsole<string> debugConsole =
				new SimpleDebugConsole<string>(null, new List<ICommand<string>> {mockCommand.Object});
			debugConsole.ExecuteCommand(alias, null);
			mockCommand.Verify(c => c.Execute(null, null), Times.Once);
		}

		[Test]
		public void AddCommand_AddCommandWithOneAlias_AliasExecutes()
		{
			const string mockName = "name";
			const string alias = "alias";
			var mockCommand = A.MockCommand(mockName).WithAlias(alias);
			IDebugConsole<string> debugConsole = new SimpleDebugConsole<string>(null, new List<ICommand<string>>());
			debugConsole.AddCommand(mockCommand.Object);
			debugConsole.ExecuteCommand(alias, null);
			mockCommand.Verify(c => c.Execute(null, null), Times.Once);
		}
	}
}
