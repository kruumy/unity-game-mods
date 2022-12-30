using System;
using HeathenEngineering.SteamApi.Foundation;
using Steamworks;

namespace HeathenEngineering.SteamApi.Networking;

public class SteamworksLobbyMember
{
	public CSteamID lobbyId;

	public SteamUserData userData;

	[Obsolete("Metadata member is no longer used on the SteamworksLobbyMember object, please use the string indexer [string metadataKey] to access a specific metadata field.", true)]
	public SteamworksLobbyMetadata Metadata { get; }

	public string this[string metadataKey]
	{
		get
		{
			return SteamMatchmaking.GetLobbyMemberData(lobbyId, userData.id, metadataKey);
		}
		set
		{
			SteamMatchmaking.SetLobbyMemberData(lobbyId, metadataKey, value);
		}
	}

	public bool IsReady
	{
		get
		{
			return this["z_heathenReady"] == "true";
		}
		set
		{
			this["z_heathenReady"] = value.ToString().ToLower();
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

	public SteamworksLobbyMember(CSteamID lobbyId, CSteamID userId)
	{
		this.lobbyId = lobbyId;
		userData = SteamSettings.current.client.GetUserData(userId);
	}
}
