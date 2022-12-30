using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.PlayerServices;

[CreateAssetMenu(menuName = "Steamworks/Player Services/Leaderboard Data")]
public class SteamworksLeaderboardData : ScriptableObject
{
	public bool createIfMissing;

	public ELeaderboardSortMethod sortMethod;

	public ELeaderboardDisplayType displayType;

	public string leaderboardName;

	public int MaxDetailEntries;

	[HideInInspector]
	public SteamLeaderboard_t? LeaderboardId;

	[HideInInspector]
	public LeaderboardEntry_t? UserEntry;

	public UnityEvent BoardFound = new UnityEvent();

	public LeaderboardScoresDownloadedEvent OnQueryResults = new LeaderboardScoresDownloadedEvent();

	public UnityLeaderboardRankUpdateEvent UserRankLoaded = new UnityLeaderboardRankUpdateEvent();

	public UnityLeaderboardRankChangeEvent UserRankChanged = new UnityLeaderboardRankChangeEvent();

	public UnityLeaderboardRankChangeEvent UserNewHighRank = new UnityLeaderboardRankChangeEvent();

	private CallResult<LeaderboardFindResult_t> OnLeaderboardFindResultCallResult;

	private CallResult<LeaderboardScoresDownloaded_t> OnLeaderboardScoresDownloadedCallResult;

	private CallResult<LeaderboardScoreUploaded_t> OnLeaderboardScoreUploadedCallResult;

	public void Register()
	{
		OnLeaderboardFindResultCallResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
		OnLeaderboardScoresDownloadedCallResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnLeaderboardScoresDownloaded);
		OnLeaderboardScoreUploadedCallResult = CallResult<LeaderboardScoreUploaded_t>.Create(OnLeaderboardScoreUploaded);
		if (createIfMissing)
		{
			FindOrCreateLeaderboard(sortMethod, displayType);
		}
		else
		{
			FindLeaderboard();
		}
	}

	private void FindOrCreateLeaderboard(ELeaderboardSortMethod sortMethod, ELeaderboardDisplayType displayType)
	{
		SteamAPICall_t hAPICall = SteamUserStats.FindOrCreateLeaderboard(leaderboardName, sortMethod, displayType);
		OnLeaderboardFindResultCallResult.Set(hAPICall);
	}

	private void FindLeaderboard()
	{
		SteamAPICall_t hAPICall = SteamUserStats.FindLeaderboard(leaderboardName);
		OnLeaderboardFindResultCallResult.Set(hAPICall);
	}

	public void RefreshUserEntry()
	{
		if (!LeaderboardId.HasValue)
		{
			Debug.LogError(base.name + " Leaderboard Data Object, cannot download scores, the leaderboard has not been initalized and cannot download scores.");
			return;
		}
		CSteamID[] prgUsers = new CSteamID[1] { SteamUser.GetSteamID() };
		SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntriesForUsers(LeaderboardId.Value, prgUsers, 1);
		OnLeaderboardScoresDownloadedCallResult.Set(hAPICall, OnLeaderboardUserRefreshRequest);
	}

	public void UploadScore(int score, ELeaderboardUploadScoreMethod method)
	{
		if (!LeaderboardId.HasValue)
		{
			Debug.LogError(base.name + " Leaderboard Data Object, cannot upload scores, the leaderboard has not been initalized and cannot upload scores.");
			return;
		}
		SteamAPICall_t hAPICall = SteamUserStats.UploadLeaderboardScore(LeaderboardId.Value, method, score, null, 0);
		OnLeaderboardScoreUploadedCallResult.Set(hAPICall);
		Debug.Log("UPLOAD DU SCORE SANS DETAIL");
	}

	public void UploadScore(int score, int[] scoreDetails, ELeaderboardUploadScoreMethod method)
	{
		if (!LeaderboardId.HasValue)
		{
			Debug.LogError(base.name + " Leaderboard Data Object, cannot upload scores, the leaderboard has not been initalized and cannot upload scores.");
			return;
		}
		SteamAPICall_t hAPICall = SteamUserStats.UploadLeaderboardScore(LeaderboardId.Value, method, score, scoreDetails, scoreDetails.Length);
		OnLeaderboardScoreUploadedCallResult.Set(hAPICall);
	}

	public void QueryTopEntries(int count)
	{
		QueryEntries(ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 0, count);
	}

	public void QueryFriendEntries(int aroundPlayer)
	{
		QueryEntries(ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends, -aroundPlayer, aroundPlayer);
	}

	public void QueryPeerEntries(int aroundPlayer)
	{
		QueryEntries(ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -aroundPlayer, aroundPlayer);
	}

	public void QueryEntries(ELeaderboardDataRequest requestType, int rangeStart, int rangeEnd)
	{
		if (!LeaderboardId.HasValue)
		{
			Debug.LogError(base.name + " Leaderboard Data Object, cannot download scores, the leaderboard has not been initalized and cannot download scores.");
			return;
		}
		SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntries(LeaderboardId.Value, requestType, rangeStart, rangeEnd);
		OnLeaderboardScoresDownloadedCallResult.Set(hAPICall, OnLeaderboardScoresDownloaded);
	}

	private void OnLeaderboardScoreUploaded(LeaderboardScoreUploaded_t param, bool bIOFailure)
	{
		if (param.m_bSuccess == 0 || bIOFailure)
		{
			Debug.LogError(base.name + " Leaderboard Data Object, failed to upload score to Steam: Success code = " + param.m_bSuccess, this);
		}
		RefreshUserEntry();
	}

	private void OnLeaderboardUserRefreshRequest(LeaderboardScoresDownloaded_t param, bool bIOFailure)
	{
		ProcessScoresDownloaded(param, bIOFailure);
	}

	private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t param, bool bIOFailure)
	{
		bool playerIncluded = ProcessScoresDownloaded(param, bIOFailure);
		OnQueryResults.Invoke(new LeaderboardScoresDownloaded
		{
			bIOFailure = bIOFailure,
			scoreData = param,
			playerIncluded = playerIncluded
		});
	}

	private bool ProcessScoresDownloaded(LeaderboardScoresDownloaded_t param, bool bIOFailure)
	{
		bool result = false;
		if (!bIOFailure)
		{
			CSteamID steamID = SteamUser.GetSteamID();
			for (int i = 0; i < param.m_cEntryCount; i++)
			{
				int[] array = null;
				LeaderboardEntry_t pLeaderboardEntry;
				if (MaxDetailEntries < 1)
				{
					SteamUserStats.GetDownloadedLeaderboardEntry(param.m_hSteamLeaderboardEntries, i, out pLeaderboardEntry, array, MaxDetailEntries);
				}
				else
				{
					array = new int[MaxDetailEntries];
					SteamUserStats.GetDownloadedLeaderboardEntry(param.m_hSteamLeaderboardEntries, i, out pLeaderboardEntry, array, MaxDetailEntries);
				}
				if (pLeaderboardEntry.m_steamIDUser.m_SteamID != steamID.m_SteamID)
				{
					continue;
				}
				result = true;
				if (!UserEntry.HasValue || UserEntry.Value.m_nGlobalRank != pLeaderboardEntry.m_nGlobalRank)
				{
					LeaderboardUserData leaderboardUserData = default(LeaderboardUserData);
					leaderboardUserData.leaderboardName = leaderboardName;
					leaderboardUserData.leaderboardId = LeaderboardId.Value;
					leaderboardUserData.entry = pLeaderboardEntry;
					leaderboardUserData.details = array;
					LeaderboardUserData arg = leaderboardUserData;
					LeaderboardRankChangeData leaderboardRankChangeData = default(LeaderboardRankChangeData);
					leaderboardRankChangeData.leaderboardName = leaderboardName;
					leaderboardRankChangeData.leaderboardId = LeaderboardId.Value;
					leaderboardRankChangeData.newEntry = pLeaderboardEntry;
					leaderboardRankChangeData.oldEntry = (UserEntry.HasValue ? new LeaderboardEntry_t?(UserEntry.Value) : null);
					LeaderboardRankChangeData arg2 = leaderboardRankChangeData;
					UserEntry = pLeaderboardEntry;
					UserRankLoaded.Invoke(arg);
					UserRankChanged.Invoke(arg2);
					if (arg2.newEntry.m_nGlobalRank < (arg2.oldEntry.HasValue ? arg2.oldEntry.Value.m_nGlobalRank : int.MaxValue))
					{
						UserNewHighRank.Invoke(arg2);
					}
				}
				else
				{
					LeaderboardUserData leaderboardUserData = default(LeaderboardUserData);
					leaderboardUserData.leaderboardName = leaderboardName;
					leaderboardUserData.leaderboardId = LeaderboardId.Value;
					leaderboardUserData.entry = pLeaderboardEntry;
					leaderboardUserData.details = array;
					LeaderboardUserData arg3 = leaderboardUserData;
					UserEntry = pLeaderboardEntry;
					UserRankLoaded.Invoke(arg3);
				}
			}
		}
		return result;
	}

	private void OnLeaderboardFindResult(LeaderboardFindResult_t param, bool bIOFailure)
	{
		if (param.m_bLeaderboardFound == 0 || bIOFailure)
		{
			Debug.LogError("Failed to find leaderboard", this);
		}
		else if (param.m_bLeaderboardFound != 0)
		{
			LeaderboardId = param.m_hSteamLeaderboard;
			BoardFound.Invoke();
			RefreshUserEntry();
		}
	}
}
