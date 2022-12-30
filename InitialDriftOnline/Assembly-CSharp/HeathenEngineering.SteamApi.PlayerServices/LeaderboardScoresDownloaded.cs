using System;
using Steamworks;

namespace HeathenEngineering.SteamApi.PlayerServices;

[Serializable]
public struct LeaderboardScoresDownloaded
{
	public bool bIOFailure;

	public bool playerIncluded;

	public LeaderboardScoresDownloaded_t scoreData;
}
