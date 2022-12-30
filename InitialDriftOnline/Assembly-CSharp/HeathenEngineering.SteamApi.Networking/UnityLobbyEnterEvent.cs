using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public class UnityLobbyEnterEvent : UnityEvent<LobbyEnter_t>
{
}
