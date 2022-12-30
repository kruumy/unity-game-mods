using System;
using Steamworks;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public struct SteamworksLobbyFilter
{
	public string key;

	public string value;

	public ELobbyComparison method;
}
