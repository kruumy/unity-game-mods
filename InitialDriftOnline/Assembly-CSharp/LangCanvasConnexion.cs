using UnityEngine;
using UnityEngine.UI;

public class LangCanvasConnexion : MonoBehaviour
{
	public Text[] JoinButton;

	public Text ServerList;

	public Text Exit;

	private void Start()
	{
		LanguageDefinition();
	}

	public void LanguageDefinition()
	{
		Text[] joinButton = JoinButton;
		foreach (Text text in joinButton)
		{
			if (PlayerPrefs.GetString("JoinTxt") != "")
			{
				text.text = PlayerPrefs.GetString("JoinTxt");
			}
			else
			{
				text.text = "JOIN";
			}
		}
		if (PlayerPrefs.GetString("ServerListtxt") != "")
		{
			ServerList.text = PlayerPrefs.GetString("ServerListtxt");
		}
		else
		{
			ServerList.text = "SERVER LIST";
		}
		if (PlayerPrefs.GetString("Exitranslatetxt") != "")
		{
			Exit.text = PlayerPrefs.GetString("Exitranslatetxt");
		}
		else
		{
			Exit.text = "EXIT";
		}
	}

	private void Update()
	{
	}
}
