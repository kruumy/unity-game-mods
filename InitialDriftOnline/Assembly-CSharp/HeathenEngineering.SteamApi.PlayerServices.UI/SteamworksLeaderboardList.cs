using System.Collections.Generic;
using HeathenEngineering.Tools;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering.SteamApi.PlayerServices.UI;

public class SteamworksLeaderboardList : HeathenUIBehaviour
{
	public SteamworksLeaderboardData Settings;

	[Tooltip("The prototype to be spawned for each entry downloaded. This should contain a componenet derived from HeathenSteamLeaderboardEntry")]
	public GameObject entryPrototype;

	[Tooltip("The collection transform such as a Scroll Rect's 'container', this will be the 'parent' of the spawned entries.")]
	public RectTransform collection;

	[Tooltip("If true and if a scroll rect is found the players record will be scrolled to be as center of the view as possible on load of records.\n\nThis does not apply if the Override On Downloaded event is used.")]
	public bool focusPlayer = true;

	[HideInInspector]
	public List<ExtendedLeaderboardEntry> Entries;

	private ScrollRect scrollRect;

	public LeaderboardEntry_t? UserEntry
	{
		get
		{
			if (Settings != null)
			{
				return Settings.UserEntry;
			}
			return null;
		}
	}

	private void Start()
	{
		scrollRect = collection.GetComponentInParent<ScrollRect>();
		if (Settings != null)
		{
			RegisterBoard(Settings);
		}
	}

	public void RegisterBoard(SteamworksLeaderboardData data)
	{
		if (Settings != null)
		{
			Settings.OnQueryResults.RemoveListener(HandleQuerryResult);
		}
		Settings = data;
		Settings.OnQueryResults.AddListener(HandleQuerryResult);
	}

	private void HandleQuerryResult(LeaderboardScoresDownloaded scores)
	{
		float verticalNormalizedPosition = 1f;
		if (scores.bIOFailure)
		{
			Debug.LogError("Failed to download score from Steam", this);
			return;
		}
		if (Entries == null)
		{
			Entries = new List<ExtendedLeaderboardEntry>();
		}
		else
		{
			Entries.Clear();
		}
		if (collection != null)
		{
			List<GameObject> list = new List<GameObject>();
			foreach (Transform item in collection)
			{
				list.Add(item.gameObject);
			}
			while (list.Count > 0)
			{
				GameObject obj = list[0];
				list.RemoveAt(0);
				Object.Destroy(obj);
			}
		}
		CSteamID steamID = SteamUser.GetSteamID();
		for (int i = 0; i < scores.scoreData.m_cEntryCount; i++)
		{
			ExtendedLeaderboardEntry extendedLeaderboardEntry = new ExtendedLeaderboardEntry();
			LeaderboardEntry_t pLeaderboardEntry;
			if (Settings.MaxDetailEntries < 1)
			{
				SteamUserStats.GetDownloadedLeaderboardEntry(scores.scoreData.m_hSteamLeaderboardEntries, i, out pLeaderboardEntry, null, 0);
				extendedLeaderboardEntry.Base = pLeaderboardEntry;
				Debug.Log("DOWNLOAD PLAYER SCORE WITHOUT DETAIL");
			}
			else
			{
				int[] array = new int[Settings.MaxDetailEntries];
				SteamUserStats.GetDownloadedLeaderboardEntry(scores.scoreData.m_hSteamLeaderboardEntries, i, out pLeaderboardEntry, array, Settings.MaxDetailEntries);
				extendedLeaderboardEntry.Base = pLeaderboardEntry;
				extendedLeaderboardEntry.Details = array;
				Debug.Log("DOWNLOAD PLAYER SCORE WITH DETAIL");
			}
			Entries.Add(extendedLeaderboardEntry);
			if (focusPlayer && steamID.m_SteamID == pLeaderboardEntry.m_steamIDUser.m_SteamID)
			{
				verticalNormalizedPosition = (float)i / (float)scores.scoreData.m_cEntryCount;
			}
			if (entryPrototype != null && collection != null)
			{
				HeathenSteamLeaderboardEntry component = Object.Instantiate(entryPrototype, collection).GetComponent<HeathenSteamLeaderboardEntry>();
				if (component != null)
				{
					component.selfTransform.localPosition = Vector3.zero;
					component.selfTransform.localRotation = Quaternion.identity;
					component.selfTransform.localScale = Vector3.one;
					component.ApplyEntry(extendedLeaderboardEntry);
				}
			}
		}
		if (focusPlayer && scrollRect != null)
		{
			Canvas.ForceUpdateCanvases();
			scrollRect.verticalNormalizedPosition = verticalNormalizedPosition;
			Canvas.ForceUpdateCanvases();
		}
	}

	public void UploadScore(int score, ELeaderboardUploadScoreMethod method)
	{
		Settings.UploadScore(score, method);
	}

	public void UploadScore(int score, int[] scoreDetails, ELeaderboardUploadScoreMethod method)
	{
		Settings.UploadScore(score, scoreDetails, method);
	}

	public void QueryEntries(ELeaderboardDataRequest requestType, int rangeStart, int rangeEnd)
	{
		Settings.QueryEntries(requestType, rangeStart, rangeEnd);
	}

	public void QueryTopEntries(int count)
	{
		Settings.QueryTopEntries(count);
	}

	public void QueryPeerEntries(int aroundPlayer)
	{
		Settings.QueryPeerEntries(aroundPlayer);
	}

	public void QueryFriendEntries(int aroundPlayer)
	{
		Settings.QueryFriendEntries(aroundPlayer);
	}

	public void RefreshUserEntry()
	{
		Settings.RefreshUserEntry();
	}
}
