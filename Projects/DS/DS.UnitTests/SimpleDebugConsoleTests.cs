using System.Collections.Generic;
using DS.DebugConsole;
using Moq;
using NUnit.Framework;

namespace DS.UnitTests
{
	public class SimpleDebugConsoleTests
	{
		private const string COMMAND_NAME = "name";
		private const string COMMAND_ALIAS = "alias";
		private const string INVALID_COMMAND_NAME = "otherName";
		private IDebugConsole<string> _debugConsole;
		private Mock<ICommand<string>> _mockCommand;
		private ICommand<string> _command;

		[SetUp]
		public void SetUp()
		{
			_debugConsole = new SimpleDebugConsole<string>(null, new List<ICommand<string>>());
			_mockCommand = A.MockCommand(COMMAND_NAME);
			_command = _mockCommand.Object;
		}

		#region Constructor

		[Test]
		public void Constructor_GivenNoCommands_ReturnsEmptyList()
		{
			Assert.IsNotNull(_debugConsole.Commands);
			Assert.IsEmpty(_debugConsole.Commands);
		}

		[Test]
		public void Constructor_GivenCommand_HasCommand()
		{
			GivenConsoleConstructedWithCommand();
			Assert.IsNotEmpty(_debugConsole.Commands);
			HasCommand();
		}

		#endregion

		#region Commands

		[Test]
		public void Commands_GivenCommandInList_HasCommand()
		{
			var commands = new List<ICommand<string>> {_command};
			_debugConsole.Commands = commands;
			HasCommand();
		}

		#endregion

		#region AddCommand

		[Test]
		public void AddCommand_GivenCommand_HasCommand()
		{
			GivenCommand();
			HasCommand();
		}

		[Test]
		public void AddCommand_GivenRepeatedCommand_HasCommandOnce()
		{
			GivenCommand();
			GivenCommand();
			Assert.IsNotEmpty(_debugConsole.Commands);
			Assert.AreEqual(1, _debugConsole.Commands.Count);
		}

		#endregion

		#region IsValidCommand

		[Test]
		public void IsValidCommand_GivenCommand_IsValidCommand()
		{
			GivenCommand();
			IsValidCommand(COMMAND_NAME);
		}

		[Test]
		public void IsValidCommand_GivenCommandWithAlias_AliasIsValidCommand()
		{
			GivenCommandWithAlias();
			IsValidCommand(COMMAND_ALIAS);
		}

		[Test]
		public void IsValidCommand_GivenInvalidCommandName_ReturnsFalse()
		{
			bool isValid = _debugConsole.IsValidCommand(INVALID_COMMAND_NAME);
			Assert.IsFalse(isValid);
		}

		#endregion

		#region ExecuteCommand

		[Test]
		public void ExecuteCommand_GivenInvalidCommandName_DoesNotExecute()
		{
			GivenCommand();
			_debugConsole.ExecuteCommand(INVALID_COMMAND_NAME, null);
			CommandExecutes(Times.Never());
		}

		[Test]
		public void ExecuteCommand_GivenCommand_ExecutesOnce()
		{
			GivenCommand();
			_debugConsole.ExecuteCommand(COMMAND_NAME, null);
			CommandExecutes(Times.Once());
		}

		[Test]
		public void ExecuteCommand_GivenCommandWithAlias_Executes()
		{
			GivenCommandWithAlias();
			_debugConsole.ExecuteCommand(COMMAND_ALIAS, null);
			CommandExecutes(Times.Once());
		}

		#endregion

		#region Given

		private void GivenConsoleConstructedWithCommand()
		{
			_debugConsole = new SimpleDebugConsole<string>(null, new List<ICommand<string>> {_command});
		}

		private void GivenCommand()
		{
			_debugConsole.AddCommand(_command);
		}

		private void GivenCommandWithAlias()
		{
			_mockCommand = A.MockCommand(COMMAND_NAME).WithAlias(COMMAND_ALIAS);
			_command = _mockCommand.Object;
			_debugConsole.AddCommand(_command);
		}

		#endregion

		#region Assert

		private void HasCommand()
		{
			Assert.AreEqual(COMMAND_NAME, _debugConsole.Commands[0].Name);
		}

		private void IsValidCommand(string commandName)
		{
			Assert.IsTrue(_debugConsole.IsValidCommand(commandName));
		}

		private void CommandExecutes(Times times)
		{
			_mockCommand.Verify(c => c.Execute(null, null), times);
		}

		#endregion
	}
}