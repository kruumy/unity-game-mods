using System.Collections.Generic;
using System.Linq;
using Steamworks;
using UnityEngine;

namespace HeathenEngineering.SteamApi.PlayerServices;

public class SteamworksLeaderboardManager : MonoBehaviour
{
	public List<SteamworksLeaderboardData> Leaderboards;

	public UnityLeaderboardRankChangeEvent LeaderboardRankChanged;

	public UnityLeaderboardRankUpdateEvent LeaderboardRankLoaded;

	public UnityLeaderboardRankChangeEvent LeaderboardNewHighRank;

	private static SteamworksLeaderboardManager Instance;

	private void Start()
	{
		if (Instance != null)
		{
			Debug.LogWarning("[SteamworksLeaderboardManager.Start] Detected a possible duplicate Steamworks Leaderboard Manager, this may cause unexpected behaviour.");
		}
		Instance = this;
	}

	private void OnEnable()
	{
		foreach (SteamworksLeaderboardData leaderboard in Leaderboards)
		{
			leaderboard.Register();
			leaderboard.UserRankChanged.AddListener(HandleLeaderboardRankChanged);
			leaderboard.UserRankLoaded.AddListener(HandleLeaderboardRankLoaded);
			leaderboard.UserNewHighRank.AddListener(HandleLeaderboardNewHighRank);
		}
	}

	private void HandleLeaderboardRankLoaded(LeaderboardUserData arg0)
	{
		LeaderboardRankLoaded.Invoke(arg0);
	}

	private void HandleLeaderboardRankChanged(LeaderboardRankChangeData arg0)
	{
		LeaderboardRankChanged.Invoke(arg0);
	}

	private void HandleLeaderboardNewHighRank(LeaderboardRankChangeData arg0)
	{
		LeaderboardNewHighRank.Invoke(arg0);
	}

	public SteamworksLeaderboardData GetLeaderboard(string name)
	{
		return Leaderboards.FirstOrDefault((SteamworksLeaderboardData p) => p.leaderboardName == name);
	}

	public SteamworksLeaderboardData GetLeaderboard(LeaderboardRankChangeData chageData)
	{
		return Leaderboards.FirstOrDefault((SteamworksLeaderboardData p) => p.leaderboardName == chageData.leaderboardName);
	}

	public void UploadLeaderboardScore(string boardName, int score, ELeaderboardUploadScoreMethod method)
	{
		SteamworksLeaderboardData steamworksLeaderboardData = Leaderboards.FirstOrDefault((SteamworksLeaderboardData p) => p.leaderboardName == boardName);
		if (steamworksLeaderboardData != null)
		{
			steamworksLeaderboardData.UploadScore(score, method);
		}
		else
		{
			Debug.LogError("[SteamworksLeaderboardManager.UploadLeaderboardScore] Unable to locate leaderboard [" + boardName + "], make sure the board is referenced in the Steamworks Leaderboard Manager.");
		}
	}

	public void UploadLeaderboardScore(int boardIndex, int score, ELeaderboardUploadScoreMethod method)
	{
		if (boardIndex > 0 && boardIndex < Leaderboards.Count)
		{
			SteamworksLeaderboardData steamworksLeaderboardData = Leaderboards[boardIndex];
			if (steamworksLeaderboardData != null)
			{
				steamworksLeaderboardData.UploadScore(score, method);
			}
		}
		else
		{
			Debug.LogError("[SteamworksLeaderboardManager.UploadLeaderboardScore] boardIndex is out of bounds, the value must be greater than 0 and less than Leaderboards.Count");
		}
	}

	public void UploadLeaderboardScore(SteamworksLeaderboardData leaderboard, int score, ELeaderboardUploadScoreMethod method)
	{
		if (leaderboard != null)
		{
			leaderboard.UploadScore(score, method);
		}
		else
		{
			Debug.LogError("[SteamworksLeaderboardManager.UploadLeaderboardScore] Leaderboard is null, no score will be uploaded.");
		}
	}

	public static void _UploadLeaderboardScore(string boardName, int score, ELeaderboardUploadScoreMethod method)
	{
		if (!(Instance == null))
		{
			SteamworksLeaderboardData steamworksLeaderboardData = Instance.Leaderboards.FirstOrDefault((SteamworksLeaderboardData p) => p.leaderboardName == boardName);
			if (steamworksLeaderboardData != null)
			{
				steamworksLeaderboardData.UploadScore(score, method);
			}
			else
			{
				Debug.LogError("Unable to locate leaderboard [" + boardName + "], make sure the board is referenced in the Heathen Steam Manager.");
			}
		}
	}

	public static void _UploadLeaderboardScore(int boardIndex, int score, ELeaderboardUploadScoreMethod method)
	{
		if (Instance == null)
		{
			return;
		}
		if (boardIndex > 0 && boardIndex < Instance.Leaderboards.Count)
		{
			SteamworksLeaderboardData steamworksLeaderboardData = Instance.Leaderboards[boardIndex];
			if (steamworksLeaderboardData != null)
			{
				steamworksLeaderboardData.UploadScore(score, method);
			}
		}
		else
		{
			Debug.LogError("boardIndex is out of bounds, the value must be greater than 0 and less than Leaderboards.Count");
		}
	}

	public static void _UploadLeaderboardScore(SteamworksLeaderboardData leaderboard, int score, ELeaderboardUploadScoreMethod method)
	{
		if (!(Instance == null))
		{
			Instance.UploadLeaderboardScore(leaderboard, score, method);
		}
	}
}
