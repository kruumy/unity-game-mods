using System;
using Steamworks;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public struct LobbyHunterStringFilter
{
	public string key;

	public string value;

	public ELobbyComparison method;
}
