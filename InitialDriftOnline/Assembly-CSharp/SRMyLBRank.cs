using CodeStage.AntiCheat.Storage;
using HeathenEngineering.SteamApi.Foundation;
using HeathenEngineering.SteamApi.PlayerServices;
using UnityEngine;
using UnityEngine.UI;

public class SRMyLBRank : MonoBehaviour
{
	public SteamUserData userData;

	[Space]
	public Text MyRank;

	[Space]
	public Text ScoreText;

	[Space]
	public GameObject TopLBcontent;

	public Image UsedCars;

	public string PPUsedCars;

	private int jack = 10;

	private void Start()
	{
	}

	public void takemyrank()
	{
		MyRank.text = TopLBcontent.GetComponentInChildren<BasicLeaderboardEntry>().rank.text ?? "";
		UsedCars.gameObject.SetActive(value: false);
		if (TopLBcontent.GetComponentInChildren<BasicLeaderboardEntry>().score.text == "999")
		{
			if ((bool)TopLBcontent.GetComponentInChildren<BasicLeaderboardEntry>().apo)
			{
				ScoreText.text = "N/A 's";
			}
			else
			{
				ScoreText.text = "N/A";
			}
		}
		else if ((bool)TopLBcontent.GetComponentInChildren<BasicLeaderboardEntry>().apo)
		{
			UsedCars.gameObject.SetActive(value: true);
			UsedCars.sprite = Object.FindObjectOfType<SRUIManager>().CarsIcon[ObscuredPrefs.GetInt(PPUsedCars)];
			ScoreText.text = TopLBcontent.GetComponentInChildren<BasicLeaderboardEntry>().score.text + " 's";
		}
		else
		{
			ScoreText.text = TopLBcontent.GetComponentInChildren<BasicLeaderboardEntry>().score.text;
		}
	}

	private void Update()
	{
		if ((bool)TopLBcontent.GetComponentInChildren<BasicLeaderboardEntry>() && jack == 10 && TopLBcontent.GetComponentInChildren<BasicLeaderboardEntry>().avatar.personaName.text == userData.DisplayName)
		{
			jack = 20;
			takemyrank();
		}
	}
}
