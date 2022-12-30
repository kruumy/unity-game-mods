using System;
using Steamworks;
using UnityEngine.Events;

namespace HeathenEngineering.SteamApi.GameServices;

[Serializable]
public class UnityWorkshopRemoveAppDependencyResultEvent : UnityEvent<RemoveAppDependencyResult_t>
{
}
