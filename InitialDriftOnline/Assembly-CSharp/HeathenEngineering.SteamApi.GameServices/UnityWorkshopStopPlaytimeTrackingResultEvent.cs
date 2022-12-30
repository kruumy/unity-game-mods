using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopStopPlaytimeTrackingResultEvent : UnityEvent<StopPlaytimeTrackingResult_t>
{
}
