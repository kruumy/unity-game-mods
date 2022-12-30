using HeathenEngineering.Events;
using HeathenEngineering.SteamApi.Foundation;
using HeathenEngineering.SteamApi.Networking.UI;
using UnityEngine;

namespace HeathenEngineering.SteamApi.Networking.Demo;

public class CommandHandler : MonoBehaviour
{
	public SteamSettings steamSettings;

	public SteamworksLobbyChat lobbyChat;

	public GameEvent sayMyNameEvent;

	public StringGameEvent echoThisEvent;

	private void Start()
	{
		sayMyNameEvent.AddListener(SayMyName);
		echoThisEvent.AddListener(echoThisMessage);
	}

	private void echoThisMessage(EventData<string> message)
	{
		lobbyChat.SendSystemMessage("Heathen Engineer", string.Concat("You want me to say \"", message, "\"\nOkay ", message.value.ToUpper(), "!!!"));
	}

	private void SayMyName(EventData data)
	{
		lobbyChat.SendSystemMessage("Heathen Engineer", "Your name is " + steamSettings.client.userData.DisplayName);
	}
}
