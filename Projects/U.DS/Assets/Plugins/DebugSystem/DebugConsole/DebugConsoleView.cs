using System.Collections.Generic;
using DS.DebugConsole;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.DebugSystem.DebugConsole
{
	public class DebugConsoleView : MonoBehaviour
	{
		[SerializeField] private Text consoleBody;
		[SerializeField] private InputField inputField;
		[SerializeField] private DebugConsoleControllerSO debugConsoleController;
		private IDebugConsole<string> _debugConsole;
		private List<ICommand<string>> _commands;
		public void ReadInput(string input)
		{
			if(!Input.GetButtonDown("Submit"))
				return;
			_ = debugConsoleController.TryUseInput(input);
		}

		private void Start()
		{
			debugConsoleController.onFeedback += WriteFeedback;
		}

		private void WriteFeedback(string newFeedBack)
		{
			consoleBody.text += "\n" + newFeedBack;
			inputField.ActivateInputField();
		}
	}
}