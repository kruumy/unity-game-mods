using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Networking;

[Serializable]
public class UnityLobbyChatUpdateEvent : UnityEvent<LobbyChatUpdate_t>
{
}
