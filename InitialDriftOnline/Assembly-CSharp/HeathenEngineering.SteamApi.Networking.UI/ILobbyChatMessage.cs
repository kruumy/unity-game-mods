namespace HeathenEngineering.SteamApi.Networking.UI;

public interface ILobbyChatMessage
{
	void RegisterChatMessage(LobbyChatMessageData data);

	void SetMessageText(string sender, string message);
}
