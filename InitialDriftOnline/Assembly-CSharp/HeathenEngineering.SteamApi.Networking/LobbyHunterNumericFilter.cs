using System;
using Steamworks;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public struct LobbyHunterNumericFilter
{
	public string key;

	public int value;

	public ELobbyComparison method;
}
