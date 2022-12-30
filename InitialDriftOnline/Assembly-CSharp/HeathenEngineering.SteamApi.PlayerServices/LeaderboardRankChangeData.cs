using Steamworks;

namespace HeathenEngineering.SteamApi.PlayerServices;

public struct LeaderboardRankChangeData
{
	public string leaderboardName;

	public SteamLeaderboard_t leaderboardId;

	public LeaderboardEntry_t? oldEntry;

	public LeaderboardEntry_t newEntry;

	public int rankDelta
	{
		get
		{
			if (oldEntry.HasValue)
			{
				return newEntry.m_nGlobalRank - oldEntry.Value.m_nGlobalRank;
			}
			return newEntry.m_nGlobalRank;
		}
	}

	public int scoreDeta
	{
		get
		{
			if (oldEntry.HasValue)
			{
				return newEntry.m_nScore - oldEntry.Value.m_nScore;
			}
			return newEntry.m_nScore;
		}
	}
}
