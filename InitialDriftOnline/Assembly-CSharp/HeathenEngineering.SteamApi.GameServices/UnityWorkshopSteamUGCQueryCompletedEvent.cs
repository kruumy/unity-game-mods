using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopSteamUGCQueryCompletedEvent : UnityEvent<SteamUGCQueryCompleted_t>
{
}
