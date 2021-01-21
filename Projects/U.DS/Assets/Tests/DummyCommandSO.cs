using System;
using System.Collections.Generic;
using Plugins.DebugSystem.DebugConsole.Commands;
using UnityEngine;

namespace Tests
{
	[CreateAssetMenu(menuName = "Debug Console/Commands/Dummy", fileName = "DummyCommand")]
	public class DummyCommandSO : CommandSO
	{
		[SerializeField] private string myName;
		[SerializeField] private string myDescription;
		[SerializeField] private string[] myAliases;
		public override void Execute(string[] args, Action<string> giveFeedBack)
		{
			giveFeedBack($"you called the <color=green>{myName}</color> command");
			if (args.Length == 0)
				return;
			var argsFeedback = "args are: ";
			foreach (var arg in args)
			{
				argsFeedback += $"{arg}	";
			}
			giveFeedBack(argsFeedback);
		}

		public override string Name => myName;
		public override IEnumerable<string> Aliases => myAliases;
		public override string Description => myDescription;
	}
}