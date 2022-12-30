using System;
using System.Collections.Generic;
using Steamworks;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public struct LobbyHunterLobbyRecord
{
	public string name;

	public CSteamID lobbyId;

	public int maxSlots;

	public CSteamID hostId;

	public Dictionary<string, string> metadata;
}
