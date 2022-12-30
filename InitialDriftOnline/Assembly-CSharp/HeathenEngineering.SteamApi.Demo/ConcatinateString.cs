using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.Demo;

public class ConcatinateString : MonoBehaviour
{
	public Text output;

	public InputField input;

	public void Concat()
	{
		Text text = output;
		text.text = text.text + "\n" + input.text;
	}
}
