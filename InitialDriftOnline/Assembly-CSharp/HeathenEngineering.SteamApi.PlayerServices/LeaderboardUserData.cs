using Steamworks;

namespace HeathenEngineering.SteamApi.PlayerServices;

public struct LeaderboardUserData
{
	public string leaderboardName;

	public SteamLeaderboard_t leaderboardId;

	public LeaderboardEntry_t entry;

	public int[] details;
}
