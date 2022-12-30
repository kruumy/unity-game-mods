using HeathenEngineering.SteamApi.Foundation;
using HeathenEngineering.SteamApi.Foundation.UI;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.PlayerServices;

public class BasicLeaderboardEntry : HeathenSteamLeaderboardEntry
{
	public Text rank;

	public SteamUserFullIcon avatar;

	public string formatString;

	public Text score;

	public LeaderboardEntry_t data;

	public Text apo;

	public Sprite[] carsicon;

	public Image TheUsedCars;

	public override void ApplyEntry(ExtendedLeaderboardEntry entry)
	{
		data = entry.Base;
		SteamUserData userData = SteamSettings.current.client.GetUserData(entry.Base.m_steamIDUser);
		avatar.LinkSteamUser(userData);
		if (!string.IsNullOrEmpty(formatString))
		{
			score.text = entry.Base.m_nScore.ToString(formatString);
		}
		else
		{
			score.text = entry.Base.m_nScore.ToString();
		}
		if (score.text == "999")
		{
			score.text = "N/A";
		}
		rank.text = entry.Base.m_nGlobalRank.ToString();
		carsicon = Object.FindObjectOfType<SRUIManager>().CarsIcon;
		int num = 999;
		if ((bool)TheUsedCars)
		{
			num = entry.Details[0];
			if (num < 50 && score.text != "N/A")
			{
				TheUsedCars.gameObject.SetActive(value: true);
				TheUsedCars.sprite = carsicon[num];
			}
			else
			{
				TheUsedCars.gameObject.SetActive(value: false);
			}
		}
	}
}
