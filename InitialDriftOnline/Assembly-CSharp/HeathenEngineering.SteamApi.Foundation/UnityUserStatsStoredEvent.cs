using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.Foundation;

[Serializable]
public class UnityUserStatsStoredEvent : UnityEvent<UserStatsStored_t>
{
}
