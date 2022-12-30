using System;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Foundation;

[CreateAssetMenu(menuName = "Steamworks/Foundation/Achievement Data")]
public class SteamAchievementData : ScriptableObject
{
	public string achievementId;

	[NonSerialized]
	public bool isAchieved;

	[NonSerialized]
	public string displayName;

	[NonSerialized]
	public string displayDescription;

	[NonSerialized]
	public bool hidden;

	public UnityEvent OnUnlock;

	public void Unlock()
	{
		if (!isAchieved)
		{
			isAchieved = true;
			SteamUserStats.SetAchievement(achievementId);
			OnUnlock.Invoke();
		}
	}

	public void ClearAchievement()
	{
		isAchieved = false;
		SteamUserStats.ClearAchievement(achievementId);
	}
}
