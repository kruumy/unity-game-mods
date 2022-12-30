using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public class SteamLobby
{
	public const string DataName = "name";

	public const string DataVersion = "z_heathenGameVersion";

	public const string DataReady = "z_heathenReady";

	public const string DataKick = "z_heathenKick";

	public CSteamID id;

	private SteamworksLobbyMember previousOwner;

	public readonly List<SteamworksLobbyMember> members = new List<SteamworksLobbyMember>();

	[HideInInspector]
	public UnityEvent LobbyDataUpdateFailed = new UnityEvent();

	[HideInInspector]
	public UnityLobbyEvent OnExitLobby = new UnityLobbyEvent();

	[HideInInspector]
	public SteamworksSteamIDEvent OnKickedFromLobby = new SteamworksSteamIDEvent();

	[HideInInspector]
	public UnityGameLobbyJoinRequestedEvent OnGameLobbyJoinRequest = new UnityGameLobbyJoinRequestedEvent();

	[HideInInspector]
	public SteamworksLobbyMemberEvent OnOwnershipChange = new SteamworksLobbyMemberEvent();

	[HideInInspector]
	public SteamworksLobbyMemberEvent OnMemberJoined = new SteamworksLobbyMemberEvent();

	[HideInInspector]
	public SteamworksLobbyMemberEvent OnMemberLeft = new SteamworksLobbyMemberEvent();

	[HideInInspector]
	public SteamworksLobbyMemberEvent OnMemberDataChanged = new SteamworksLobbyMemberEvent();

	[HideInInspector]
	public UnityEvent OnLobbyDataChanged = new UnityEvent();

	[HideInInspector]
	public UnityLobbyGameCreatedEvent OnGameServerSet = new UnityLobbyGameCreatedEvent();

	[HideInInspector]
	public UnityLobbyChatUpdateEvent OnLobbyChatUpdate = new UnityLobbyChatUpdateEvent();

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

	private bool p_joinable = true;

	public SteamworksLobbyMember Owner
	{
		get
		{
			CSteamID ownerId = SteamMatchmaking.GetLobbyOwner(id);
			SteamworksLobbyMember steamworksLobbyMember = members.FirstOrDefault((SteamworksLobbyMember p) => p.userData != null && p.userData.id == ownerId);
			if (steamworksLobbyMember == null)
			{
				steamworksLobbyMember = new SteamworksLobbyMember(id, ownerId);
				members.Add(steamworksLobbyMember);
			}
			return steamworksLobbyMember;
		}
		set
		{
			if (value.lobbyId == id && value.userData != null)
			{
				SteamMatchmaking.SetLobbyOwner(id, value.userData.id);
			}
		}
	}

	public CSteamID OwnerId
	{
		get
		{
			return Owner.userData.id;
		}
		set
		{
			if (members.Any((SteamworksLobbyMember p) => p.userData != null && p.userData.id == value))
			{
				SteamMatchmaking.SetLobbyOwner(id, value);
			}
		}
	}

	public SteamworksLobbyMember User
	{
		get
		{
			CSteamID userId = SteamUser.GetSteamID();
			if (id == CSteamID.Nil)
			{
				return null;
			}
			SteamworksLobbyMember steamworksLobbyMember = members.FirstOrDefault((SteamworksLobbyMember p) => p.userData != null && p.userData.id == userId);
			if (steamworksLobbyMember != null)
			{
				return steamworksLobbyMember;
			}
			int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(id);
			for (int i = 0; i < numLobbyMembers; i++)
			{
				if (SteamMatchmaking.GetLobbyMemberByIndex(id, i) == userId)
				{
					steamworksLobbyMember = new SteamworksLobbyMember(id, userId);
					members.Add(steamworksLobbyMember);
					return steamworksLobbyMember;
				}
			}
			return null;
		}
	}

	public string Name
	{
		get
		{
			return this["name"];
		}
		set
		{
			this["name"] = value;
		}
	}

	public string GameVersion
	{
		get
		{
			return this["z_heathenGameVersion"];
		}
		set
		{
			this["z_heathenGameVersion"] = value;
		}
	}

	public int MemberCountLimit
	{
		get
		{
			return SteamMatchmaking.GetLobbyMemberLimit(id);
		}
		set
		{
			SteamMatchmaking.SetLobbyMemberLimit(id, value);
		}
	}

	public int MemberCount => members.Count;

	public bool Joinable
	{
		get
		{
			return p_joinable;
		}
		set
		{
			if (SteamMatchmaking.SetLobbyJoinable(id, value))
			{
				p_joinable = value;
			}
		}
	}

	public LobbyGameServerInformation GameServer { get; private set; }

	public bool IsHost => SteamUser.GetSteamID() == SteamMatchmaking.GetLobbyOwner(id);

	public bool HasGameServer
	{
		get
		{
			uint punGameServerIP;
			ushort punGameServerPort;
			CSteamID psteamIDGameServer;
			bool lobbyGameServer = SteamMatchmaking.GetLobbyGameServer(id, out punGameServerIP, out punGameServerPort, out psteamIDGameServer);
			GameServer = new LobbyGameServerInformation
			{
				ipAddress = punGameServerIP,
				port = punGameServerPort,
				serverId = psteamIDGameServer
			};
			return lobbyGameServer;
		}
	}

	public string this[string metadataKey]
	{
		get
		{
			return SteamMatchmaking.GetLobbyData(id, metadataKey);
		}
		set
		{
			SteamMatchmaking.SetLobbyData(id, metadataKey, value);
		}
	}

	public bool AllPlayersReady
	{
		get
		{
			if (!members.Any((SteamworksLobbyMember p) => !p.IsReady))
			{
				return true;
			}
			return false;
		}
	}

	public bool AllPlayersNotReady
	{
		get
		{
			if (!members.Any((SteamworksLobbyMember p) => p.IsReady))
			{
				return true;
			}
			return false;
		}
	}

	public SteamLobby(CSteamID lobbyId)
	{
		id = lobbyId;
		int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(id);
		for (int i = 0; i < numLobbyMembers; i++)
		{
			CSteamID lobbyMemberByIndex = SteamMatchmaking.GetLobbyMemberByIndex(id, i);
			members.Add(new SteamworksLobbyMember(id, lobbyMemberByIndex));
		}
		previousOwner = Owner;
	}

	public int GetMetadataCount()
	{
		return SteamMatchmaking.GetLobbyDataCount(id);
	}

	public Dictionary<string, string> GetMetadataEntries()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int lobbyDataCount = SteamMatchmaking.GetLobbyDataCount(id);
		for (int i = 0; i < lobbyDataCount; i++)
		{
			SteamMatchmaking.GetLobbyDataByIndex(id, i, out var pchKey, 255, out var pchValue, 8192);
			dictionary.Add(pchKey, pchValue);
		}
		return dictionary;
	}

	internal void HandleLobbyGameCreated(LobbyGameCreated_t param)
	{
		GameServer = new LobbyGameServerInformation
		{
			ipAddress = param.m_unIP,
			port = param.m_usPort,
			serverId = new CSteamID(param.m_ulSteamIDGameServer)
		};
		OnGameServerSet.Invoke(param);
	}

	internal void HandleLobbyChatUpdate(LobbyChatUpdate_t pCallback)
	{
		if (id.m_SteamID != pCallback.m_ulSteamIDLobby)
		{
			return;
		}
		if (pCallback.m_rgfChatMemberStateChange == 2)
		{
			CSteamID memberId = new CSteamID(pCallback.m_ulSteamIDUserChanged);
			SteamworksLobbyMember steamworksLobbyMember = members.FirstOrDefault((SteamworksLobbyMember p) => p.userData != null && p.userData.id == memberId);
			if (steamworksLobbyMember != null)
			{
				members.Remove(steamworksLobbyMember);
				OnMemberLeft.Invoke(steamworksLobbyMember);
				ChatMemberStateChangeLeft.Invoke(steamworksLobbyMember.userData);
			}
		}
		else if (pCallback.m_rgfChatMemberStateChange == 1)
		{
			CSteamID memberId2 = new CSteamID(pCallback.m_ulSteamIDUserChanged);
			SteamworksLobbyMember steamworksLobbyMember2 = members.FirstOrDefault((SteamworksLobbyMember p) => p.userData != null && p.userData.id == memberId2);
			if (steamworksLobbyMember2 == null)
			{
				steamworksLobbyMember2 = new SteamworksLobbyMember(id, memberId2);
				members.Add(steamworksLobbyMember2);
				OnMemberJoined.Invoke(steamworksLobbyMember2);
			}
			ChatMemberStateChangeEntered.Invoke(steamworksLobbyMember2);
		}
		else if (pCallback.m_rgfChatMemberStateChange == 4)
		{
			CSteamID memberId3 = new CSteamID(pCallback.m_ulSteamIDUserChanged);
			SteamworksLobbyMember steamworksLobbyMember3 = members.FirstOrDefault((SteamworksLobbyMember p) => p.userData != null && p.userData.id == memberId3);
			if (steamworksLobbyMember3 != null)
			{
				members.Remove(steamworksLobbyMember3);
				OnMemberLeft.Invoke(steamworksLobbyMember3);
				ChatMemberStateChangeDisconnected.Invoke(steamworksLobbyMember3.userData);
			}
		}
		else if (pCallback.m_rgfChatMemberStateChange == 8)
		{
			CSteamID memberId4 = new CSteamID(pCallback.m_ulSteamIDUserChanged);
			SteamworksLobbyMember steamworksLobbyMember4 = members.FirstOrDefault((SteamworksLobbyMember p) => p.userData != null && p.userData.id == memberId4);
			if (steamworksLobbyMember4 != null)
			{
				members.Remove(steamworksLobbyMember4);
				OnMemberLeft.Invoke(steamworksLobbyMember4);
				ChatMemberStateChangeKicked.Invoke(steamworksLobbyMember4.userData);
			}
		}
		else if (pCallback.m_rgfChatMemberStateChange == 16)
		{
			CSteamID memberId5 = new CSteamID(pCallback.m_ulSteamIDUserChanged);
			SteamworksLobbyMember steamworksLobbyMember5 = members.FirstOrDefault((SteamworksLobbyMember p) => p.userData != null && p.userData.id == memberId5);
			if (steamworksLobbyMember5 != null)
			{
				members.Remove(steamworksLobbyMember5);
				OnMemberLeft.Invoke(steamworksLobbyMember5);
				ChatMemberStateChangeBanned.Invoke(steamworksLobbyMember5.userData);
			}
		}
		OnLobbyChatUpdate.Invoke(pCallback);
		SteamworksLobbyMember owner = Owner;
		if (previousOwner != owner)
		{
			previousOwner = owner;
			OnOwnershipChange.Invoke(owner);
		}
	}

	internal void HandleLobbyDataUpdate(LobbyDataUpdate_t param)
	{
		bool flag = false;
		if (param.m_bSuccess == 0)
		{
			LobbyDataUpdateFailed.Invoke();
			return;
		}
		if (param.m_ulSteamIDLobby == param.m_ulSteamIDMember)
		{
			if (SteamMatchmaking.GetLobbyData(id, "z_heathenKick").Contains("[" + SteamUser.GetSteamID().m_SteamID + "]"))
			{
				Debug.Log("User has been kicked from the lobby.");
				flag = true;
			}
			OnLobbyDataChanged.Invoke();
		}
		else
		{
			CSteamID userId = new CSteamID(param.m_ulSteamIDMember);
			SteamworksLobbyMember steamworksLobbyMember = members.FirstOrDefault((SteamworksLobbyMember p) => p.userData != null && p.userData.id == userId);
			if (steamworksLobbyMember == null)
			{
				steamworksLobbyMember = new SteamworksLobbyMember(id, userId);
				members.Add(steamworksLobbyMember);
				OnMemberJoined.Invoke(steamworksLobbyMember);
			}
		}
		if (flag)
		{
			CSteamID arg = id;
			Leave();
			OnKickedFromLobby.Invoke(arg);
			return;
		}
		SteamworksLobbyMember owner = Owner;
		if (previousOwner != owner)
		{
			previousOwner = owner;
			OnOwnershipChange.Invoke(owner);
		}
	}

	internal LobbyChatMessageData HandleLobbyChatMessage(LobbyChatMsg_t pCallback)
	{
		CSteamID cSteamID = (CSteamID)pCallback.m_ulSteamIDLobby;
		if (cSteamID != id)
		{
			return null;
		}
		byte[] array = new byte[4096];
		CSteamID SteamIDUser;
		EChatEntryType peChatEntryType;
		int lobbyChatEntry = SteamMatchmaking.GetLobbyChatEntry(cSteamID, (int)pCallback.m_iChatID, out SteamIDUser, array, array.Length, out peChatEntryType);
		byte[] array2 = new byte[lobbyChatEntry];
		Array.Copy(array, array2, lobbyChatEntry);
		LobbyChatMessageData lobbyChatMessageData = new LobbyChatMessageData
		{
			sender = members.FirstOrDefault((SteamworksLobbyMember p) => p.userData.id == SteamIDUser),
			message = Encoding.UTF8.GetString(array2),
			recievedTime = DateTime.Now,
			chatEntryType = peChatEntryType,
			lobby = this
		};
		OnChatMessageReceived.Invoke(lobbyChatMessageData);
		return lobbyChatMessageData;
	}

	public void Leave()
	{
		if (!(id == CSteamID.Nil))
		{
			try
			{
				SteamMatchmaking.LeaveLobby(id);
			}
			catch
			{
			}
			OnExitLobby.Invoke(this);
			id = CSteamID.Nil;
			members.Clear();
		}
	}

	public bool DeleteLobbyData(string dataKey)
	{
		return SteamMatchmaking.DeleteLobbyData(id, dataKey);
	}

	public bool InviteUserToLobby(CSteamID targetUser)
	{
		return SteamMatchmaking.InviteUserToLobby(id, targetUser);
	}

	public bool SendChatMessage(string message)
	{
		if (string.IsNullOrEmpty(message))
		{
			return false;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(message);
		return SteamMatchmaking.SendLobbyChatMsg(id, bytes, bytes.Length);
	}

	public void SetGameServer(string address, ushort port, CSteamID gameServerId)
	{
		GameServer = new LobbyGameServerInformation
		{
			port = port,
			StringAddress = address,
			serverId = gameServerId
		};
		SteamMatchmaking.SetLobbyGameServer(id, GameServer.ipAddress, port, gameServerId);
	}

	public void SetGameServer(string address, ushort port)
	{
		GameServer = new LobbyGameServerInformation
		{
			port = port,
			StringAddress = address,
			serverId = CSteamID.Nil
		};
		SteamMatchmaking.SetLobbyGameServer(id, GameServer.ipAddress, port, CSteamID.Nil);
	}

	public void SetGameServer(CSteamID gameServerId)
	{
		GameServer = new LobbyGameServerInformation
		{
			port = 0,
			ipAddress = 0u,
			serverId = gameServerId
		};
		SteamMatchmaking.SetLobbyGameServer(id, 0u, 0, gameServerId);
	}

	public void SetGameServer()
	{
		GameServer = new LobbyGameServerInformation
		{
			port = 0,
			ipAddress = 0u,
			serverId = SteamUser.GetSteamID()
		};
		SteamMatchmaking.SetLobbyGameServer(id, 0u, 0, GameServer.serverId);
	}

	public bool SetLobbyType(ELobbyType type)
	{
		return SteamMatchmaking.SetLobbyType(id, type);
	}

	public void KickMember(CSteamID memberId)
	{
		if (!IsHost)
		{
			Debug.LogError("Only the host of a lobby can kick a member from it.");
			return;
		}
		if (memberId.m_SteamID == SteamUser.GetSteamID().m_SteamID)
		{
			Leave();
			OnKickedFromLobby.Invoke(SteamUser.GetSteamID());
			return;
		}
		Debug.Log("Marking " + memberId.m_SteamID + " for removal");
		string text = SteamMatchmaking.GetLobbyData(id, "z_heathenKick");
		if (text == null)
		{
			text = string.Empty;
		}
		if (!text.Contains("[" + memberId.ToString() + "]"))
		{
			text = text + "[" + memberId.ToString() + "]";
		}
		SteamMatchmaking.SetLobbyData(id, "z_heathenKick", text);
	}
}
