using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopAddDependencyResultEvent : UnityEvent<AddUGCDependencyResult_t>
{
}
