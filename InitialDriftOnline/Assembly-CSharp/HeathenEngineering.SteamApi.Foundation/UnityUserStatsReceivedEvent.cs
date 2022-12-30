using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
public class UnityUserStatsReceivedEvent : UnityEvent<UserStatsReceived_t>
{
}
