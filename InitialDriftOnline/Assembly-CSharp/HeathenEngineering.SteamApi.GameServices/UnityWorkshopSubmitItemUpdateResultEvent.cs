using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopSubmitItemUpdateResultEvent : UnityEvent<SubmitItemUpdateResult_t>
{
}
