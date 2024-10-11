using System.Collections.Generic;
using System.Data;
using System.Linq;
using DS.DebugConsole;
using Moq;
using NUnit.Framework;

namespace DS.UnitTests
{
	public class SimpleDebugConsoleTests
	{
		private const string CommandName = "name";
		private const string CommandAlias = "alias";
		private const string InvalidCommandName = "otherName";
		private IDebugConsole<string> _debugConsole;
		private Mock<ICommand<string>> _mockCommand;
		private ICommand<string> _command;

		[SetUp]
		public void SetUp()
		{
			_debugConsole = new SimpleDebugConsole<string>(null);
			_mockCommand = A.MockCommand(CommandName);
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
			VerifyThatHasCommand();
		}

		#endregion

		#region Commands

		[Test]
		public void Commands_GivenCommandInList_HasCommand()
		{
			var commands = new HashSet<ICommand<string>> {_command};
			_debugConsole.Commands = commands;
			VerifyThatHasCommand();
		}

		#endregion

		#region AddCommand

		[Test]
		public void AddCommand_GivenCommand_HasCommand()
		{
			GivenACommand();
			VerifyThatHasCommand();
		}

		[Test]
		public void AddCommand_GivenDuplicateCommand_ThrowsDuplicateException()
		{
			GivenACommand();
			Assert.Throws<DuplicateNameException>(GivenACommand);
		}
		
		[Test]
		public void AddCommand_GivenDuplicateCommand_HasCommandOnce()
		{
			GivenACommand();
			try
			{
				GivenACommand();
			}
			catch { /*ignored*/ }

			VerifyThatHasCommand();
			Assert.AreEqual(1, _debugConsole.Commands.Count);
		}

		#endregion
		
		#region TryAddCommand

		[Test]
		public void TryAddCommand_GivenCommand_ReturnsTrue()
		{
			Assert.IsTrue(_debugConsole.TryAddCommand(_command));
		}
		
		[Test]
		public void TryAddCommand_GivenCommand_HasCommand()
		{
			_debugConsole.TryAddCommand(_command);
			VerifyThatHasCommand();
		}

		[Test]
		public void TryAddCommand_GivenDuplicateCommand_ReturnsFalse()
		{
			GivenACommand();
			Assert.IsFalse(_debugConsole.TryAddCommand(_command));
		}
		
		[Test]
		public void TryAddCommand_GivenDuplicateCommand_HasCommandOnce()
		{
			GivenACommand();
			_debugConsole.TryAddCommand(_command);

			VerifyThatHasCommand();
			Assert.AreEqual(1, _debugConsole.Commands.Count);
		}

		#endregion

		#region IsValidCommand

		[Test]
		public void IsValidCommand_GivenCommand_IsValidCommand()
		{
			GivenACommand();
			VerifyThatIsValidCommand(CommandName);
		}

		[Test]
		public void IsValidCommand_GivenCommandWithAlias_AliasIsValidCommand()
		{
			GivenCommandWithAlias();
			VerifyThatIsValidCommand(CommandAlias);
		}

		[Test]
		public void IsValidCommand_GivenInvalidCommandName_ReturnsFalse()
		{
			bool isValid = _debugConsole.IsValidCommand(InvalidCommandName);
			Assert.IsFalse(isValid);
		}

		#endregion

		#region ExecuteCommand

		[Test]
		public void ExecuteCommand_GivenInvalidCommandName_DoesNotExecute()
		{
			GivenACommand();
			_debugConsole.ExecuteCommand(InvalidCommandName);
			VerifyThatCommandExecutes(Times.Never());
		}

		[Test]
		public void ExecuteCommand_GivenCommand_ExecutesOnce()
		{
			GivenACommand();
			_debugConsole.ExecuteCommand(CommandName);
			VerifyThatCommandExecutes(Times.Once());
		}

		[Test]
		public void ExecuteCommand_GivenCommandWithAlias_Executes()
		{
			GivenCommandWithAlias();
			_debugConsole.ExecuteCommand(CommandAlias);
			VerifyThatCommandExecutes(Times.Once());
		}

		#endregion

		#region Given

		private void GivenConsoleConstructedWithCommand()
		{
			_debugConsole = new SimpleDebugConsole<string>(null, _command);
		}

		private void GivenACommand()
		{
			_debugConsole.AddCommand(_command);
		}

		private void GivenCommandWithAlias()
		{
			_mockCommand = A.MockCommand(CommandName).WithAlias(CommandAlias);
			_command = _mockCommand.Object;
			_debugConsole.AddCommand(_command);
		}

		#endregion

		#region Assert

		private void VerifyThatHasCommand()
		{
			Assert.IsNotEmpty(_debugConsole.Commands);
			Assert.Contains(_command, _debugConsole.Commands.ToArray());
			var firstValue = _debugConsole.Commands.FirstOrDefault();
			Assert.IsNotNull(firstValue);
			Assert.AreEqual(CommandName, firstValue.Name);
		}

		private void VerifyThatIsValidCommand(string commandName)
		{
			Assert.IsTrue(_debugConsole.IsValidCommand(commandName));
		}

		private void VerifyThatCommandExecutes(Times times)
		{
			_mockCommand.Verify(c => c.Execute(null), times);
		}

		#endregion
	}
}