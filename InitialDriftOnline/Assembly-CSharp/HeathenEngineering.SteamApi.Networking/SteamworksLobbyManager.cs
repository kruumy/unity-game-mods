using System;
using HeathenEngineering.SteamApi.Foundation;
using HeathenEngineering.Tools;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Networking;

public class SteamworksLobbyManager : HeathenBehaviour, ISteamworksLobbyManager
{
	public SteamworksLobbySettings LobbySettings;

	public UnityGameLobbyJoinRequestedEvent OnGameLobbyJoinRequest = new UnityGameLobbyJoinRequestedEvent();

	public UnityLobbyHunterListEvent OnLobbyMatchList = new UnityLobbyHunterListEvent();

	public UnityLobbyCreatedEvent OnLobbyCreated = new UnityLobbyCreatedEvent();

	public UnityLobbyEvent OnLobbyEnter = new UnityLobbyEvent();

	public UnityLobbyEvent OnLobbyExit = new UnityLobbyEvent();

	public UnityLobbyGameCreatedEvent OnGameServerSet = new UnityLobbyGameCreatedEvent();

	public UnityLobbyChatUpdateEvent OnLobbyChatUpdate = new UnityLobbyChatUpdateEvent();

	public UnityEvent QuickMatchFailed = new UnityEvent();

	public UnityEvent SearchStarted = new UnityEvent();

	public LobbyChatMessageEvent OnChatMessageReceived = new LobbyChatMessageEvent();

	public SteamworksLobbyMemberEvent ChatMemberStateChangeEntered = new SteamworksLobbyMemberEvent();

	public UnityPersonaEvent ChatMemberStateChangeLeft = new UnityPersonaEvent();

	public UnityPersonaEvent ChatMemberStateChangeDisconnected = new UnityPersonaEvent();

	public UnityPersonaEvent ChatMemberStateChangeKicked = new UnityPersonaEvent();

	public UnityPersonaEvent ChatMemberStateChangeBanned = new UnityPersonaEvent();

	public bool IsSearching => LobbySettings.IsSearching;

	public bool IsQuickSearching => LobbySettings.IsQuickSearching;

	private void OnEnable()
	{
		if (LobbySettings == null)
		{
			Debug.LogWarning("Lobby settings not found ... creating default settings");
			LobbySettings = ScriptableObject.CreateInstance<SteamworksLobbySettings>();
		}
		else if (LobbySettings.Manager != null && LobbySettings.Manager != this)
		{
			Debug.LogWarning("Lobby settings already references a manager, this lobby manager will overwrite it. Please insure there is only 1 SteamworksLobbyManager active at a time.");
		}
		LobbySettings.Manager = this;
		LobbySettings.Initalize();
		LobbySettings.OnGameLobbyJoinRequest.AddListener(OnGameLobbyJoinRequest.Invoke);
		LobbySettings.OnLobbyMatchList.AddListener(OnLobbyMatchList.Invoke);
		LobbySettings.OnLobbyCreated.AddListener(OnLobbyCreated.Invoke);
		LobbySettings.OnLobbyExit.AddListener(OnLobbyExit.Invoke);
		LobbySettings.OnLobbyEnter.AddListener(OnLobbyEnter.Invoke);
		LobbySettings.OnGameServerSet.AddListener(OnGameServerSet.Invoke);
		LobbySettings.OnLobbyChatUpdate.AddListener(OnLobbyChatUpdate.Invoke);
		LobbySettings.QuickMatchFailed.AddListener(QuickMatchFailed.Invoke);
		LobbySettings.SearchStarted.AddListener(SearchStarted.Invoke);
		LobbySettings.OnChatMessageReceived.AddListener(OnChatMessageReceived.Invoke);
		LobbySettings.ChatMemberStateChangeEntered.AddListener(ChatMemberStateChangeEntered.Invoke);
		LobbySettings.ChatMemberStateChangeLeft.AddListener(ChatMemberStateChangeLeft.Invoke);
		LobbySettings.ChatMemberStateChangeDisconnected.AddListener(ChatMemberStateChangeDisconnected.Invoke);
		LobbySettings.ChatMemberStateChangeKicked.AddListener(ChatMemberStateChangeKicked.Invoke);
		LobbySettings.ChatMemberStateChangeBanned.AddListener(ChatMemberStateChangeBanned.Invoke);
	}

	private void OnDestroy()
	{
		try
		{
			if (LobbySettings != null && LobbySettings.Manager == this)
			{
				LobbySettings.Manager = null;
				LobbySettings.OnGameLobbyJoinRequest.RemoveListener(OnGameLobbyJoinRequest.Invoke);
				LobbySettings.OnLobbyMatchList.RemoveListener(OnLobbyMatchList.Invoke);
				LobbySettings.OnLobbyCreated.RemoveListener(OnLobbyCreated.Invoke);
				LobbySettings.OnLobbyExit.RemoveListener(OnLobbyExit.Invoke);
				LobbySettings.OnLobbyEnter.RemoveListener(OnLobbyEnter.Invoke);
				LobbySettings.OnGameServerSet.RemoveListener(OnGameServerSet.Invoke);
				LobbySettings.OnLobbyChatUpdate.RemoveListener(OnLobbyChatUpdate.Invoke);
				LobbySettings.QuickMatchFailed.RemoveListener(QuickMatchFailed.Invoke);
				LobbySettings.SearchStarted.RemoveListener(SearchStarted.Invoke);
				LobbySettings.OnChatMessageReceived.RemoveListener(OnChatMessageReceived.Invoke);
				LobbySettings.ChatMemberStateChangeEntered.RemoveListener(ChatMemberStateChangeEntered.Invoke);
				LobbySettings.ChatMemberStateChangeLeft.RemoveListener(ChatMemberStateChangeLeft.Invoke);
				LobbySettings.ChatMemberStateChangeDisconnected.RemoveListener(ChatMemberStateChangeDisconnected.Invoke);
				LobbySettings.ChatMemberStateChangeKicked.RemoveListener(ChatMemberStateChangeKicked.Invoke);
				LobbySettings.ChatMemberStateChangeBanned.RemoveListener(ChatMemberStateChangeBanned.Invoke);
			}
		}
		catch
		{
		}
	}

	[Obsolete("CreateLobby(LobbyHunterFilter lobbyFilter, string lobbyName, ELobbyType lobbyType) is deprecated, please use CreateLobby(ELobbyType lobbyType, int memberCountLimit) instead.", true)]
	public void CreateLobby(LobbyHunterFilter LobbyFilter, string LobbyName = "", ELobbyType lobbyType = ELobbyType.k_ELobbyTypePublic)
	{
		throw new NotImplementedException();
	}

	[Obsolete("LeaveLobby is deprecated, please use the Leave method available on the SteamLobby object to leave a specific lobby, e.g. LobbySettings.lobbies[0].Leave();", true)]
	public void LeaveLobby()
	{
		throw new NotImplementedException();
	}

	public void CreateLobby(ELobbyType lobbyType, int memberCountLimit)
	{
		LobbySettings.CreateLobby(lobbyType, memberCountLimit);
	}

	public void JoinLobby(CSteamID lobbyId)
	{
		LobbySettings.JoinLobby(lobbyId);
	}

	public void FindMatch(LobbyHunterFilter LobbyFilter)
	{
		if (LobbySettings != null)
		{
			LobbySettings.FindMatch(LobbyFilter);
		}
		else
		{
			Debug.LogWarning("[HeatehnSteamLobbyManager|FindMatch] attempted to find a match while [HeathenSteamLobbyManager|LobbySettings] is null");
		}
	}

	public bool QuickMatch(LobbyHunterFilter LobbyFilter, string onCreateName, bool autoCreate = false)
	{
		if (LobbySettings != null)
		{
			return LobbySettings.QuickMatch(LobbyFilter, onCreateName, autoCreate);
		}
		Debug.LogWarning("[HeatehnSteamLobbyManager|QuickMatch] attempted to quick match while [HeathenSteamLobbyManager|LobbySettings] is null");
		return false;
	}

	public void CancelQuickMatch()
	{
		if (LobbySettings != null)
		{
			LobbySettings.CancelQuickMatch();
		}
		else
		{
			Debug.LogWarning("[HeatehnSteamLobbyManager|CancelQuickMatch] attempted to cancel a quick match search while [HeathenSteamLobbyManager|LobbySettings] is null");
		}
	}

	public void CancelStandardSearch()
	{
		LobbySettings.CancelStandardSearch();
	}

	public void SendChatMessage(string message)
	{
		LobbySettings.SendChatMessage(message);
	}

	public void SetLobbyMetadata(string key, string value)
	{
		LobbySettings.SetLobbyMetadata(key, value);
	}

	public void SetMemberMetadata(string key, string value)
	{
		LobbySettings.SetMemberMetadata(key, value);
	}

	public void SetLobbyGameServer()
	{
		LobbySettings.SetLobbyGameServer();
	}

	public void SetLobbyGameServer(string address, ushort port, CSteamID steamID)
	{
		LobbySettings.SetLobbyGameServer(address, port, steamID);
	}
}
