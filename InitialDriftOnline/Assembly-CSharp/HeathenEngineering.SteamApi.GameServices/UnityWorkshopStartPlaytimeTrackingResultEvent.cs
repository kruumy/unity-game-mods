using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopStartPlaytimeTrackingResultEvent : UnityEvent<StartPlaytimeTrackingResult_t>
{
}
