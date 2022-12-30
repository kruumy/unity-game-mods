using Steamworks;

namespace HeathenEngineering.SteamApi.Networking;

public interface ISteamworksLobbyManager
{
	bool IsSearching { get; }

	bool IsQuickSearching { get; }

	void CreateLobby(LobbyHunterFilter LobbyFilter, string LobbyName = "", ELobbyType lobbyType = ELobbyType.k_ELobbyTypePublic);

	void JoinLobby(CSteamID lobbyId);

	void LeaveLobby();

	void FindMatch(LobbyHunterFilter LobbyFilter);

	bool QuickMatch(LobbyHunterFilter LobbyFilter, string onCreateName, bool autoCreate = false);

	void CancelQuickMatch();

	void CancelStandardSearch();

	void SendChatMessage(string message);

	void SetLobbyMetadata(string key, string value);

	void SetMemberMetadata(string key, string value);

	void SetLobbyGameServer();

	void SetLobbyGameServer(string address, ushort port, CSteamID steamID);
}
