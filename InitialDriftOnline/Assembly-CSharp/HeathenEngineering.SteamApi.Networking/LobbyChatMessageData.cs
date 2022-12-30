using System;
using Steamworks;

namespace HeathenEngineering.SteamApi.Networking;

public class LobbyChatMessageData
{
	public EChatEntryType chatEntryType;

	public SteamLobby lobby;

	public SteamworksLobbyMember sender;

	public DateTime recievedTime;

	public string message;
}
