using Plugins.DebugSystem;
using UnityEngine;

public class ConsoleTest : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			FindObjectOfType<GeneralInformationView>().AddInformation(GetRandomInfo);
		}
		else if (Input.GetKeyDown(KeyCode.F3))
		{
			FindObjectOfType<GeneralInformationView>().RemoveInformation(GetRandomInfo);
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			FindObjectOfType<GeneralInformationView>().AddInformation(GetRandomInfo2);
		}
		else if (Input.GetKeyDown(KeyCode.F4))
		{
			FindObjectOfType<GeneralInformationView>().RemoveInformation(GetRandomInfo2);
		}
	}

	private string GetRandomInfo() => $"<color=green>test:</color> time = <color=red>{Time.time}</color>";
	private string GetRandomInfo2() => $"<color=green>test:</color> deltaTime = <color=red>{Time.deltaTime}</color>";
}