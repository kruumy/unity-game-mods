using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
[CreateAssetMenu(menuName = "Steamworks/Networking/Lobby Settings")]
public class SteamworksLobbySettings : ScriptableObject
{
	public readonly List<SteamLobby> lobbies = new List<SteamLobby>();

	[Header("Quick Match Settings")]
	[FormerlySerializedAs("MaxDistanceFilter")]
	public ELobbyDistanceFilter maxDistanceFilter = ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault;

	[HideInInspector]
	public ISteamworksLobbyManager Manager;

	[NonSerialized]
	private bool standardSearch;

	[NonSerialized]
	private bool quickMatchSearch;

	[NonSerialized]
	private bool callbacksRegistered;

	private LobbyHunterFilter createLobbyFilter;

	private LobbyHunterFilter quickMatchFilter;

	private CallResult<LobbyCreated_t> m_LobbyCreated;

	private Callback<LobbyEnter_t> m_LobbyEntered;

	private Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;

	private Callback<LobbyChatUpdate_t> m_LobbyChatUpdate;

	private CallResult<LobbyMatchList_t> m_LobbyMatchList;

	private Callback<LobbyGameCreated_t> m_LobbyGameCreated;

	private Callback<LobbyDataUpdate_t> m_LobbyDataUpdated;

	private Callback<LobbyChatMsg_t> m_LobbyChatMsg;

	[HideInInspector]
	public UnityGameLobbyJoinRequestedEvent OnGameLobbyJoinRequest = new UnityGameLobbyJoinRequestedEvent();

	[HideInInspector]
	public UnityLobbyHunterListEvent OnLobbyMatchList = new UnityLobbyHunterListEvent();

	[HideInInspector]
	public UnityLobbyCreatedEvent OnLobbyCreated = new UnityLobbyCreatedEvent();

	[HideInInspector]
	public UnityLobbyEvent OnLobbyEnter = new UnityLobbyEvent();

	[HideInInspector]
	public UnityLobbyEvent OnLobbyExit = new UnityLobbyEvent();

	[HideInInspector]
	public UnityLobbyGameCreatedEvent OnGameServerSet = new UnityLobbyGameCreatedEvent();

	[HideInInspector]
	public UnityLobbyChatUpdateEvent OnLobbyChatUpdate = new UnityLobbyChatUpdateEvent();

	[HideInInspector]
	public UnityEvent QuickMatchFailed = new UnityEvent();

	[HideInInspector]
	public UnityEvent SearchStarted = new UnityEvent();

	[HideInInspector]
	public LobbyChatMessageEvent OnChatMessageReceived = new LobbyChatMessageEvent();

	[HideInInspector]
	public SteamworksLobbyMemberEvent ChatMemberStateChangeEntered = new SteamworksLobbyMemberEvent();

	[HideInInspector]
	public UnityPersonaEvent ChatMemberStateChangeLeft = new UnityPersonaEvent();

	[HideInInspector]
	public UnityPersonaEvent ChatMemberStateChangeDisconnected = new UnityPersonaEvent();

	[HideInInspector]
	public UnityPersonaEvent ChatMemberStateChangeKicked = new UnityPersonaEvent();

	[HideInInspector]
	public UnityPersonaEvent ChatMemberStateChangeBanned = new UnityPersonaEvent();

	public SteamLobby this[CSteamID lobbyId] => lobbies.FirstOrDefault((SteamLobby p) => p.id == lobbyId);

	public bool InLobby
	{
		get
		{
			if (lobbies.Any((SteamLobby p) => p != null && p.User != null))
			{
				return true;
			}
			return false;
		}
	}

	public bool HasLobby => lobbies.Any((SteamLobby p) => p.id != CSteamID.Nil);

	public bool IsSearching => standardSearch;

	public bool IsQuickSearching => quickMatchSearch;

	[Obsolete("OnLobbyDataChanged member is no longer used at the settings level, please use SteamLobby.OnLobbyDataChanged e.g. lobbySettings.lobbies[0].OnLobbyDataChanged", true)]
	[HideInInspector]
	public UnityEvent OnLobbyDataChanged { get; }

	[Obsolete("OnOwnershipChange member is no longer used at the settings level, please use SteamLobby.OnOwnershipChange e.g. lobbySettings.lobbies[0].OnOwnershipChange", true)]
	[HideInInspector]
	public SteamworksLobbyMemberEvent OnOwnershipChange { get; }

	[Obsolete("OnMemberJoined member is no longer used at the settings level, please use SteamLobby.OnMemberJoined e.g. lobbySettings.lobbies[0].OnMemberJoined", true)]
	[HideInInspector]
	public SteamworksLobbyMemberEvent OnMemberJoined { get; }

	[Obsolete("OnMemberLeft member is no longer used at the settings level, please use SteamLobby.OnMemberLeft e.g. lobbySettings.lobbies[0].OnMemberLeft", true)]
	[HideInInspector]
	public SteamworksLobbyMemberEvent OnMemberLeft { get; }

	[Obsolete("OnMemberDataChanged member is no longer used at the settings level, please use SteamLobby.OnMemberDataChanged e.g. lobbySettings.lobbies[0].OnMemberDataChanged", true)]
	[HideInInspector]
	public SteamworksLobbyMemberEvent OnMemberDataChanged { get; }

	[Obsolete("LobbyDataUpdateFailed member is no longer used at the settings level, please use SteamLobby.LobbyDataUpdateFailed e.g. lobbySettings.lobbies[0].LobbyDataUpdateFailed", true)]
	[HideInInspector]
	public UnityEvent LobbyDataUpdateFailed { get; }

	[Obsolete("OnKickedFromLobby member is no longer used at the settings level, please use SteamLobby.OnKickedFromLobby e.g. lobbySettings.lobbies[0].OnKickedFromLobby", true)]
	[HideInInspector]
	public UnityEvent OnKickedFromLobby { get; }

	[Obsolete("IsHost member is no longer used, please use SteamLobby.IsHost e.g. lobbySettings.lobbies[0].IsHost", true)]
	public bool IsHost { get; }

	[Obsolete("HasGameServer member is no longer used, please use SteamLobby.HasGameServer e.g. lobbySettings.lobbies[0].HasGameServer", true)]
	public bool HasGameServer { get; }

	[Obsolete("GameServerInformation member is no longer used, please use SteamLobby.GameServer e.g. lobbySettings.lobbies[0].GameServer", true)]
	public LobbyGameServerInformation GameServerInformation { get; }

	[Obsolete("Metadata member is no longer used on the settings object, please use SteamLobby[string metadataKey] to access a specific metadata field or use SteamLobby.GetMetadataEntries() to return an array of KeyValuePair<string key, string value> representing each field that can be iterated over such as in a foreach loop.", true)]
	public SteamworksLobbyMetadata Metadata { get; }

	public void Initalize()
	{
		if (SteamSettings.current.Initialized && !callbacksRegistered)
		{
			callbacksRegistered = true;
			m_LobbyCreated = CallResult<LobbyCreated_t>.Create(HandleLobbyCreated);
			m_LobbyEntered = Callback<LobbyEnter_t>.Create(HandleLobbyEntered);
			m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(HandleGameLobbyJoinRequested);
			m_LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(HandleLobbyChatUpdate);
			m_LobbyMatchList = CallResult<LobbyMatchList_t>.Create(HandleLobbyMatchList);
			m_LobbyGameCreated = Callback<LobbyGameCreated_t>.Create(HandleLobbyGameCreated);
			m_LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(HandleLobbyDataUpdate);
			m_LobbyChatMsg = Callback<LobbyChatMsg_t>.Create(HandleLobbyChatMessage);
		}
	}

	private void HandleLobbyList(SteamLobbyLobbyList lobbyList)
	{
		int count = lobbyList.Count;
		if (!quickMatchSearch)
		{
			return;
		}
		if (count == 0)
		{
			if (!quickMatchFilter.useDistanceFilter)
			{
				quickMatchFilter.useDistanceFilter = true;
			}
			switch (quickMatchFilter.distanceOption)
			{
			case ELobbyDistanceFilter.k_ELobbyDistanceFilterClose:
				if (maxDistanceFilter >= ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault)
				{
					quickMatchFilter.distanceOption = ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault;
					FindQuickMatch();
				}
				else
				{
					HandleQuickMatchFailed();
				}
				break;
			case ELobbyDistanceFilter.k_ELobbyDistanceFilterDefault:
				if (maxDistanceFilter >= ELobbyDistanceFilter.k_ELobbyDistanceFilterFar)
				{
					quickMatchFilter.distanceOption = ELobbyDistanceFilter.k_ELobbyDistanceFilterFar;
					FindQuickMatch();
				}
				else
				{
					HandleQuickMatchFailed();
				}
				break;
			case ELobbyDistanceFilter.k_ELobbyDistanceFilterFar:
				if (maxDistanceFilter >= ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide)
				{
					quickMatchFilter.distanceOption = ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide;
					FindQuickMatch();
				}
				else
				{
					HandleQuickMatchFailed();
				}
				break;
			case ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide:
				HandleQuickMatchFailed();
				break;
			}
		}
		else
		{
			CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(0);
			JoinLobby(lobbyByIndex);
		}
	}

	private void HandleQuickMatchFailed()
	{
		quickMatchSearch = false;
		QuickMatchFailed.Invoke();
	}

	private void FindQuickMatch()
	{
		if (!callbacksRegistered)
		{
			Initalize();
		}
		SetLobbyFilter(quickMatchFilter);
		SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();
		m_LobbyMatchList.Set(hAPICall, HandleLobbyMatchList);
		SearchStarted.Invoke();
	}

	private void SetLobbyFilter(LobbyHunterFilter LobbyFilter)
	{
		if (LobbyFilter.useSlotsAvailable)
		{
			SteamMatchmaking.AddRequestLobbyListFilterSlotsAvailable(LobbyFilter.requiredOpenSlots);
		}
		if (LobbyFilter.useDistanceFilter)
		{
			SteamMatchmaking.AddRequestLobbyListDistanceFilter(LobbyFilter.distanceOption);
		}
		if (LobbyFilter.maxResults > 0)
		{
			SteamMatchmaking.AddRequestLobbyListResultCountFilter(LobbyFilter.maxResults);
		}
		if (LobbyFilter.numberValues != null)
		{
			foreach (LobbyHunterNumericFilter numberValue in LobbyFilter.numberValues)
			{
				SteamMatchmaking.AddRequestLobbyListNumericalFilter(numberValue.key, numberValue.value, numberValue.method);
			}
		}
		if (LobbyFilter.nearValues != null)
		{
			foreach (LobbyHunterNearFilter nearValue in LobbyFilter.nearValues)
			{
				SteamMatchmaking.AddRequestLobbyListNearValueFilter(nearValue.key, nearValue.value);
			}
		}
		if (LobbyFilter.stringValues == null)
		{
			return;
		}
		foreach (LobbyHunterStringFilter stringValue in LobbyFilter.stringValues)
		{
			SteamMatchmaking.AddRequestLobbyListStringFilter(stringValue.key, stringValue.value, stringValue.method);
		}
	}

	private void HandleLobbyGameCreated(LobbyGameCreated_t param)
	{
		SteamLobby steamLobby = lobbies.FirstOrDefault((SteamLobby p) => p.id.m_SteamID == param.m_ulSteamIDLobby);
		if (steamLobby != null)
		{
			steamLobby.HandleLobbyGameCreated(param);
			OnGameServerSet.Invoke(param);
		}
	}

	private void HandleLobbyMatchList(LobbyMatchList_t pCallback, bool bIOFailure)
	{
		uint nLobbiesMatching = pCallback.m_nLobbiesMatching;
		SteamLobbyLobbyList steamLobbyLobbyList = new SteamLobbyLobbyList();
		if (nLobbiesMatching == 0)
		{
			if (quickMatchSearch)
			{
				Debug.Log("Lobby match list returned (0), refining search paramiters.");
				HandleLobbyList(steamLobbyLobbyList);
			}
			else
			{
				Debug.Log("Lobby match list returned (" + nLobbiesMatching + ")");
				standardSearch = false;
				OnLobbyMatchList.Invoke(steamLobbyLobbyList);
			}
			return;
		}
		Debug.Log("Lobby match list returned (" + nLobbiesMatching + ")");
		for (int i = 0; i < nLobbiesMatching; i++)
		{
			LobbyHunterLobbyRecord record = default(LobbyHunterLobbyRecord);
			record.metadata = new Dictionary<string, string>();
			record.lobbyId = SteamMatchmaking.GetLobbyByIndex(i);
			record.maxSlots = SteamMatchmaking.GetLobbyMemberLimit(record.lobbyId);
			int lobbyDataCount = SteamMatchmaking.GetLobbyDataCount(record.lobbyId);
			SteamLobby steamLobby = lobbies.FirstOrDefault((SteamLobby p) => p.id == record.lobbyId);
			if (steamLobby != null)
			{
				Debug.Log("Browsed our own lobby and found " + lobbyDataCount + " metadata records.");
			}
			for (int j = 0; j < lobbyDataCount; j++)
			{
				if (SteamMatchmaking.GetLobbyDataByIndex(record.lobbyId, j, out var pchKey, 255, out var pchValue, 8192))
				{
					record.metadata.Add(pchKey, pchValue);
					if (pchKey == "name")
					{
						record.name = pchValue;
					}
					if (pchKey == "OwnerID" && ulong.TryParse(pchValue, out var result))
					{
						record.hostId = new CSteamID(result);
					}
				}
			}
			steamLobbyLobbyList.Add(record);
		}
		if (quickMatchSearch)
		{
			HandleLobbyList(steamLobbyLobbyList);
			return;
		}
		standardSearch = false;
		OnLobbyMatchList.Invoke(steamLobbyLobbyList);
	}

	private void HandleLobbyChatUpdate(LobbyChatUpdate_t param)
	{
		lobbies.FirstOrDefault((SteamLobby p) => p.id.m_SteamID == param.m_ulSteamIDLobby).HandleLobbyChatUpdate(param);
		OnLobbyChatUpdate.Invoke(param);
	}

	private void HandleGameLobbyJoinRequested(GameLobbyJoinRequested_t param)
	{
		OnGameLobbyJoinRequest.Invoke(param);
	}

	private void HandleLobbyEntered(LobbyEnter_t param)
	{
		SteamLobby steamLobby = lobbies.FirstOrDefault((SteamLobby p) => p.id.m_SteamID == param.m_ulSteamIDLobby);
		if (steamLobby == null)
		{
			steamLobby = new SteamLobby(new CSteamID(param.m_ulSteamIDLobby));
			steamLobby.OnExitLobby.AddListener(HandleExitLobby);
			lobbies.Add(steamLobby);
		}
		OnLobbyEnter.Invoke(steamLobby);
	}

	private void HandleLobbyCreated(LobbyCreated_t param, bool bIOFailure)
	{
		SteamLobby steamLobby = lobbies.FirstOrDefault((SteamLobby p) => p.id.m_SteamID == param.m_ulSteamIDLobby);
		if (steamLobby == null)
		{
			steamLobby = new SteamLobby(new CSteamID(param.m_ulSteamIDLobby));
			steamLobby.OnExitLobby.AddListener(HandleExitLobby);
			lobbies.Add(steamLobby);
		}
		OnLobbyCreated.Invoke(param);
	}

	private void HandleLobbyDataUpdate(LobbyDataUpdate_t param)
	{
		lobbies.FirstOrDefault((SteamLobby p) => p.id.m_SteamID == param.m_ulSteamIDLobby).HandleLobbyDataUpdate(param);
	}

	private void HandleLobbyChatMessage(LobbyChatMsg_t param)
	{
		LobbyChatMessageData lobbyChatMessageData = lobbies.FirstOrDefault((SteamLobby p) => p.id.m_SteamID == param.m_ulSteamIDLobby).HandleLobbyChatMessage(param);
		if (lobbyChatMessageData != null)
		{
			OnChatMessageReceived.Invoke(lobbyChatMessageData);
		}
	}

	private void HandleExitLobby(SteamLobby lobby)
	{
		lobbies.RemoveAll((SteamLobby p) => p.id == lobby.id);
		OnLobbyExit.Invoke(lobby);
	}

	[Obsolete("CreateLobby(LobbyHunterFilter lobbyFilter, string lobbyName, ELobbyType lobbyType) is deprecated, please use CreateLobby(ELobbyType lobbyType, int memberCountLimit) instead.", true)]
	public void CreateLobby(LobbyHunterFilter lobbyFilter, string lobbyName, ELobbyType lobbyType)
	{
		throw new NotImplementedException();
	}

	[Obsolete("LeaveLobby is deprecated, please use the Leave method available on the SteamLobby object to leave a specific lobby, e.g. LobbySettings.lobbies[LobbyId].Leave();", true)]
	public void LeaveLobby()
	{
		throw new NotImplementedException();
	}

	public void CreateLobby(ELobbyType lobbyType, int memberCountLimit)
	{
		SteamAPICall_t hAPICall = SteamMatchmaking.CreateLobby(lobbyType, memberCountLimit);
		m_LobbyCreated.Set(hAPICall, HandleLobbyCreated);
	}

	public void JoinLobby(CSteamID lobbyId)
	{
		SteamMatchmaking.JoinLobby(lobbyId);
	}

	public void JoinLobby(ulong lobbyId)
	{
		SteamMatchmaking.JoinLobby(new CSteamID(lobbyId));
	}

	public void LeaveAllLobbies()
	{
		SteamLobby[] array = lobbies.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Leave();
		}
		lobbies.Clear();
	}

	public void FindMatch(LobbyHunterFilter LobbyFilter)
	{
		if (quickMatchSearch)
		{
			Debug.LogError("Attempted to search for a lobby while a quick search is processing. This search will be ignored, you must call CancelQuickMatch to abort a search before it completes, note that results may still come back resulting in the next match list not being as expected.");
			return;
		}
		standardSearch = true;
		SetLobbyFilter(LobbyFilter);
		SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();
		m_LobbyMatchList.Set(hAPICall, HandleLobbyMatchList);
		SearchStarted.Invoke();
	}

	public bool QuickMatch(LobbyHunterFilter LobbyFilter, string onCreateName, bool autoCreate = false)
	{
		if (!callbacksRegistered)
		{
			Initalize();
		}
		if (quickMatchSearch || standardSearch)
		{
			return false;
		}
		quickMatchSearch = true;
		quickMatchFilter = LobbyFilter;
		quickMatchFilter.distanceOption = ELobbyDistanceFilter.k_ELobbyDistanceFilterClose;
		quickMatchFilter.useDistanceFilter = true;
		FindQuickMatch();
		return true;
	}

	public void CancelQuickMatch()
	{
		if (!callbacksRegistered)
		{
			Initalize();
		}
		if (quickMatchSearch)
		{
			quickMatchSearch = false;
			Debug.LogWarning("Quick Match search has been canceled, note that results may still come back resulting in the next match list not being as expected.");
		}
	}

	public void CancelStandardSearch()
	{
		if (!callbacksRegistered)
		{
			Initalize();
		}
		if (standardSearch)
		{
			standardSearch = false;
			Debug.LogWarning("Search has been canceled, note that results may still come back resulting in the next match list not being as expected.");
		}
	}

	public void SendChatMessage(string message)
	{
		if (lobbies.Count != 0)
		{
			if (!callbacksRegistered)
			{
				Initalize();
			}
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			SteamMatchmaking.SendLobbyChatMsg(lobbies[0].id, bytes, bytes.Length);
		}
	}

	public void SendChatMessage(CSteamID lobbyId, string message)
	{
		if (lobbies.Count != 0)
		{
			if (!callbacksRegistered)
			{
				Initalize();
			}
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			SteamMatchmaking.SendLobbyChatMsg(lobbyId, bytes, bytes.Length);
		}
	}

	public void SetLobbyMetadata(string key, string value)
	{
		if (lobbies.Count != 0)
		{
			if (!callbacksRegistered)
			{
				Initalize();
			}
			lobbies[0][key] = value;
		}
	}

	public void SetLobbyMetadata(CSteamID lobbyId, string key, string value)
	{
		if (!callbacksRegistered)
		{
			Initalize();
		}
		this[lobbyId][key] = value;
	}

	public void SetMemberMetadata(string key, string value)
	{
		if (lobbies.Count != 0)
		{
			if (!callbacksRegistered)
			{
				Initalize();
			}
			lobbies[0].User[key] = value;
		}
	}

	public void SetMemberMetadata(CSteamID lobbyId, string key, string value)
	{
		if (!callbacksRegistered)
		{
			Initalize();
		}
		this[lobbyId].User[key] = value;
	}

	public void SetLobbyGameServer()
	{
		if (!callbacksRegistered)
		{
			Initalize();
		}
		lobbies[0].SetGameServer();
	}

	public void SetLobbyGameServer(CSteamID lobbyId)
	{
		if (!callbacksRegistered)
		{
			Initalize();
		}
		this[lobbyId].SetGameServer();
	}

	public void SetLobbyGameServer(string ipAddress, ushort port, CSteamID serverId)
	{
		lobbies[0].SetGameServer(ipAddress, port, serverId);
	}

	public void SetLobbyGameServer(CSteamID lobbyId, string ipAddress, ushort port, CSteamID serverId)
	{
		this[lobbyId].SetGameServer(ipAddress, port, serverId);
	}

	public void SetLobbyJoinable(bool value)
	{
		lobbies[0].Joinable = value;
	}

	public void SetLobbyJoinable(CSteamID lobbyId, bool value)
	{
		this[lobbyId].Joinable = value;
	}

	public LobbyGameServerInformation GetGameServer()
	{
		return lobbies[0].GameServer;
	}

	public LobbyGameServerInformation GetGameServer(CSteamID lobbyId)
	{
		return this[lobbyId].GameServer;
	}

	public void KickMember(CSteamID memberId)
	{
		lobbies[0].KickMember(memberId);
	}

	public void KickMember(CSteamID lobbyId, CSteamID memberId)
	{
		this[lobbyId].KickMember(memberId);
	}

	public void ChangeOwner(CSteamID newOwner)
	{
		lobbies[0].OwnerId = newOwner;
	}

	public void ChangeOwner(CSteamID lobbyId, CSteamID newOwner)
	{
		this[lobbyId].OwnerId = newOwner;
	}
}
