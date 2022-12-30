using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.UIX;

public class Console : MonoBehaviour
{
	[Tooltip("The text object log messages will be written to")]
	public int maxLines = 200;

	[Tooltip("The text object log messages will be written to")]
	public Text text;

	[Tooltip("The parent scroll rect of the text field")]
	public ScrollRect scrollRect;

	private void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		Color color;
		switch (type)
		{
		case LogType.Error:
		case LogType.Exception:
			color = Color.red;
			break;
		case LogType.Warning:
			color = Color.yellow;
			break;
		default:
			color = Color.white;
			break;
		}
		Text text = this.text;
		text.text = text.text + "\n<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + logString + "</color>";
		Canvas.ForceUpdateCanvases();
		if (this.text.cachedTextGenerator.lineCount > maxLines)
		{
			UILineInfo uILineInfo = this.text.cachedTextGenerator.lines[this.text.cachedTextGenerator.lineCount - maxLines];
			this.text.text = this.text.text.Substring(uILineInfo.startCharIdx);
		}
		Canvas.ForceUpdateCanvases();
		scrollRect.verticalNormalizedPosition = 0f;
	}
}
