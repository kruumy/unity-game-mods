using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.Foundation.Demo;

public class ExampleStatsUpdate : MonoBehaviour
{
	[FormerlySerializedAs("SteamSettings")]
	public SteamSettings steamSettings;

	[FormerlySerializedAs("StatDataObject")]
	public SteamFloatStatData statDataObject;

	[FormerlySerializedAs("WinnerAchievement")]
	public SteamAchievementData winnerAchievement;

	[FormerlySerializedAs("StatValue")]
	public Text statValue;

	[FormerlySerializedAs("WinnerAchievmentStatus")]
	public Text winnerAchievmentStatus;

	private void Update()
	{
		statValue.text = "Feet Traveled = " + statDataObject.Value;
		winnerAchievmentStatus.text = winnerAchievement.displayName + "\n" + (winnerAchievement.isAchieved ? "(Unlocked)" : "(Locked)");
	}

	public void UpdateStatValue(float amount)
	{
		statDataObject.SetFloatStat(statDataObject.Value + amount);
		steamSettings.client.StoreStatsAndAchievements();
	}

	public void GetHelp()
	{
		Application.OpenURL("https://partner.steamgames.com/doc/features/achievements");
	}

	public void GetOverlayHelp()
	{
		Application.OpenURL("https://partner.steamgames.com/doc/features/overlay");
	}

	public void OnRetrieveStatsAndAchievements()
	{
		Debug.Log("[ExampleStatsUpdate.OnRetrieveStatsAndAchievement]\nStats loaded!");
	}

	public void OnStoredStatsAndAchievements()
	{
		Debug.Log("[ExampleStatsUpdate.OnStoredStatsAndAchievements]\nStats stored!");
	}
}
