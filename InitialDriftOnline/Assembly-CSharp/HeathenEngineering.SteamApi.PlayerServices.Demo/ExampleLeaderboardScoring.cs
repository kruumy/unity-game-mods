using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices.Demo;

public class ExampleLeaderboardScoring : MonoBehaviour
{
	public SteamworksLeaderboardData leaderboardData;

	public void UpdateScore(int score)
	{
		leaderboardData.UploadScore(score, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest);
		Debug.Log("Set leaderboard: " + leaderboardData.leaderboardName + " score to: " + score + " with instruction to keep the best value (comparing current vs new)");
	}

	public void ForceUpdateScore(int score)
	{
		leaderboardData.UploadScore(score, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate);
		Debug.Log("Set leaderboard: " + leaderboardData.leaderboardName + " score to: " + score + " with instruction to overwrite the current value");
	}

	public void AddToScore(int score)
	{
		int num = (leaderboardData.UserEntry.HasValue ? leaderboardData.UserEntry.Value.m_nScore : 0);
		leaderboardData.UploadScore(num + score, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest);
		Debug.Log("Set leaderboard: " + leaderboardData.leaderboardName + " score to: " + (num + score) + " with instruction to keep the best value (comparing current vs new)");
	}

	public void GetHelp()
	{
		Application.OpenURL("https://partner.steamgames.com/doc/features/leaderboards");
	}
}
