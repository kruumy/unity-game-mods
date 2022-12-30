using System;
using System.Collections.Generic;
using Steamworks;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public class SteamLobbyLobbyList : List<LobbyHunterLobbyRecord>
{
	public Dictionary<string, string> GetLobbyMetaData(CSteamID id)
	{
		if (Exists((LobbyHunterLobbyRecord p) => p.lobbyId.m_SteamID == id.m_SteamID))
		{
			return Find((LobbyHunterLobbyRecord p) => p.lobbyId.m_SteamID == id.m_SteamID).metadata;
		}
		return new Dictionary<string, string>();
	}
}
